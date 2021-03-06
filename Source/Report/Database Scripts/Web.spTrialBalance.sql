USE [RUG]
GO
/****** Object:  StoredProcedure [Web].[spTrialBalance]    Script Date: 05/Aug/2015 10:02:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [Web].[spTrialBalance]     
    @Site VARCHAR(20) = NULL,
    @Division VARCHAR(20) = NULL,
	@AsOnDate VARCHAR(20) = NULL

AS
BEGIN

declare @DivisionCnt Int
declare @SiteCnt Int
set @DivisionCnt='0'
set @SiteCnt='0'

declare @DivisionText varchar(max)
set @DivisionText=(Select DivisionName + ',' from web.Divisions where DivisionId In (SELECT Items FROM [dbo].[Split] (@Division, ',')) FOR XML path (''))
set @DivisionText=LEFT(@DivisionText,len(@DivisionText)-1)

Declare @SiteText varchar(max)
set @SiteText=(Select SiteName + ',' from Web.Sites where SiteId In (SELECT Items FROM [dbo].[Split] (@Site, ',')) FOR XML path (''))
set @SiteText=LEFT(@SiteText,len(@SiteText)-1)

/*For the Count Division and Site*/

select @SiteCnt=count(Distinct H.SiteId),@DivisionCnt=count(Distinct H.DivisionId) from
(
SELECT max(LH.SiteId) as SiteId,max(LH.DivisionId) as  DivisionId
FROM web.Ledgers H WITH (Nolock)
LEFT JOIN web.LedgerHeaders LH WITH (Nolock) ON LH.LedgerHeaderId = H.LedgerHeaderId 
LEFT JOIN web.LedgerAccounts LA  WITH (Nolock) ON LA.LedgerAccountId = H.LedgerAccountId 
LEFT JOIN web.LedgerAccountGroups LAG  WITH (Nolock) ON LAG.LedgerAccountGroupId = LA.LedgerAccountGroupId 
WHERE LAG.LedgerAccountGroupId IS NOT NULL 
AND ( @Site is null or LH.SiteId IN (SELECT Items FROM [dbo].[Split] (@Site, ','))) 
AND ( @Division is null or LH.DivisionId IN (SELECT Items FROM [dbo].[Split] (@Division, ','))) 
AND ( @AsOnDate is null or @AsOnDate >= LH.DocDate ) 
GROUP BY LAG.LedgerAccountGroupId 
HAVING sum(isnull(H.AmtDr,0)) <> 0 OR sum(isnull(H.AmtCr,0)) <> 0
) as H




/*main Query*/
SELECT LAG.LedgerAccountGroupId, max(LAG.LedgerAccountGroupName) AS LedgerAccountGroupName, 
CASE WHEN sum(isnull(H.AmtDr,0)) - sum(isnull(H.AmtCr,0)) > 0 THEN sum(isnull(H.AmtDr,0)) - sum(isnull(H.AmtCr,0)) ELSE NULL  END AS AmtDr,
CASE WHEN sum(isnull(H.AmtDr,0)) - sum(isnull(H.AmtCr,0)) < 0 THEN abs(sum(isnull(H.AmtDr,0)) - sum(isnull(H.AmtCr,0))) ELSE NULL END AS AmtCr,    
(case when @SiteCnt>1 THEN '0' else max(LH.SiteId) end) as SiteId,
(case when @DivisionCnt>1 THEN '0' else max(LH.DivisionId) end) as DivisionId ,
'TrialBalance.rdl' AS ReportName, 'Trial Balance' AS ReportTitle, NULL AS SubReportProcList,@DivisionText as DivisionText,@SiteText As Sitetext,@AsOnDate as OnDate
FROM web.Ledgers H WITH (Nolock)
LEFT JOIN web.LedgerHeaders LH WITH (Nolock) ON LH.LedgerHeaderId = H.LedgerHeaderId 
LEFT JOIN web.LedgerAccounts LA  WITH (Nolock) ON LA.LedgerAccountId = H.LedgerAccountId 
LEFT JOIN web.LedgerAccountGroups LAG  WITH (Nolock) ON LAG.LedgerAccountGroupId = LA.LedgerAccountGroupId 
WHERE LAG.LedgerAccountGroupId IS NOT NULL 
AND ( @Site is null or LH.SiteId IN (SELECT Items FROM [dbo].[Split] (@Site, ','))) 
AND ( @Division is null or LH.DivisionId IN (SELECT Items FROM [dbo].[Split] (@Division, ','))) 
AND ( @AsOnDate is null or @AsOnDate >= LH.DocDate ) 
GROUP BY LAG.LedgerAccountGroupId 
HAVING sum(isnull(H.AmtDr,0)) <> 0 OR sum(isnull(H.AmtCr,0)) <> 0
Order By LAG.LedgerAccountGroupId
End



