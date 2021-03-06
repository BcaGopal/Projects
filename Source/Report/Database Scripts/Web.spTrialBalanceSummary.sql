USE [RUG]
GO
/****** Object:  StoredProcedure [Web].[spTrialBalanceSummary]    Script Date: 05/Aug/2015 4:52:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [Web].[spTrialBalanceSummary]     
    @Site VARCHAR(20) = NULL,
    @Division VARCHAR(20) = NULL,
	@FromDate VARCHAR(20) = NULL,
	@ToDate VARCHAR(20) = NULL
AS
BEGIN

declare @DivisionCnt Int
declare @SiteCnt Int
set @DivisionCnt='0'
set @SiteCnt='0'

Declare @SiteText varchar(max)
set @SiteText=(Select SiteName + ',' from Web.Sites where SiteId In (SELECT Items FROM [dbo].[Split] (@Site, ',')) FOR XML path (''))
set @SiteText=LEFT(@SiteText,len(@SiteText)-1)

declare @DivisionText varchar(max)
set @DivisionText=(Select DivisionName + ',' from web.Divisions where DivisionId In (SELECT Items FROM [dbo].[Split] (@Division, ',')) FOR XML path (''))
set @DivisionText=LEFT(@DivisionText,len(@DivisionText)-1)

/*  for the Site And Division Count*/
select
@DivisionCnt=count(Distinct H.DivisionId),@SiteCnt=count(Distinct H.siteId)
 From (SELECT 
Max(DivisionId) as DivisionId,Max(siteId) as SiteId
FROM
(SELECT LA.LedgerAccountGroupId,  sum(isnull(H.AmtDr,0)) - sum(isnull(H.AmtCr,0)) AS Opening,
0 AS AmtDr,0 AS  AmtCr,max(LH.SiteId) as siteId,max(LH.DivisionId) as DivisionId   
FROM web.Ledgers H WITH (Nolock)
LEFT JOIN web.LedgerHeaders LH WITH (Nolock) ON LH.LedgerHeaderId = H.LedgerHeaderId 
LEFT JOIN web.LedgerAccounts LA  WITH (Nolock) ON LA.LedgerAccountId = H.LedgerAccountId 
WHERE LA.LedgerAccountGroupId IS NOT NULL 
AND ( @Site is null or LH.SiteId IN (SELECT Items FROM [dbo].[Split] (@Site, ','))) 
AND ( @Division is null or LH.DivisionId IN (SELECT Items FROM [dbo].[Split] (@Division, ','))) 
AND ( @FromDate is null or @FromDate > LH.DocDate ) 
GROUP BY LA.LedgerAccountGroupId 
UNION ALL 

SELECT LA.LedgerAccountGroupId, 0 AS Opening,
CASE WHEN sum(isnull(H.AmtDr,0)) - sum(isnull(H.AmtCr,0)) > 0 THEN sum(isnull(H.AmtDr,0)) - sum(isnull(H.AmtCr,0)) ELSE 0 END AS AmtDr,
CASE WHEN sum(isnull(H.AmtDr,0)) - sum(isnull(H.AmtCr,0)) < 0 THEN abs(sum(isnull(H.AmtDr,0)) - sum(isnull(H.AmtCr,0))) ELSE 0 END AS AmtCr,   
Null as siteId,Null as DivisionId
FROM web.Ledgers H WITH (Nolock)
LEFT JOIN web.LedgerHeaders LH WITH (Nolock) ON LH.LedgerHeaderId = H.LedgerHeaderId 
LEFT JOIN web.LedgerAccounts LA  WITH (Nolock) ON LA.LedgerAccountId = H.LedgerAccountId 
LEFT JOIN web.LedgerAccountGroups LAG  WITH (Nolock) ON LAG.LedgerAccountGroupId = LA.LedgerAccountGroupId 
WHERE LA.LedgerAccountGroupId IS NOT NULL 
AND ( @Site is null or LH.SiteId IN (SELECT Items FROM [dbo].[Split] (@Site, ','))) 
AND ( @Division is null or LH.DivisionId IN (SELECT Items FROM [dbo].[Split] (@Division, ','))) 
AND ( @FromDate is null or @FromDate <= LH.DocDate ) 
AND ( @ToDate is null or @ToDate >= LH.DocDate ) 
GROUP BY LA.LedgerAccountGroupId 
) AS VMain
LEFT JOIN web.LedgerAccountGroups LAG  WITH (Nolock) ON LAG.LedgerAccountGroupId = VMain.LedgerAccountGroupId 
GROUP BY VMain.LedgerAccountGroupId
HAVING Sum(Isnull(VMain.Opening,0)) <> 0 OR Sum(isnull(Vmain.AmtDr,0)) <> 0 OR Sum(isnull(Vmain.AmtDr,0)) <> 0
) as H



/* main Query*/
SELECT VMain.LedgerAccountGroupId,  max(LAG.LedgerAccountGroupName) AS LedgerAccountGroupName, 
CASE WHEN abs(Sum(Isnull(VMain.Opening,0))) =0 THEN NULL ELSE abs(Sum(Isnull(VMain.Opening,0))) END AS Opening, CASE WHEN Sum(Isnull(VMain.Opening,0)) >= 0 THEN 'Dr' ELSE 'Cr' END AS OpeningType, 
CASE WHEN Sum(isnull(Vmain.AmtDr,0)) = 0 THEN NULL ELSE Sum(isnull(Vmain.AmtDr,0)) END AS AmtDr, 
CASE WHEN sum(isnull(VMain.AmtCr,0)) = 0 THEN NULL ELSE sum(isnull(VMain.AmtCr,0)) END AS AmtCr,
abs(Sum(Isnull(VMain.Opening,0)) + Sum(isnull(Vmain.AmtDr,0)) - sum(isnull(VMain.AmtCr,0))) AS Balance,
CASE WHEN ( Sum(Isnull(VMain.Opening,0)) + Sum(isnull(Vmain.AmtDr,0)) - sum(isnull(VMain.AmtCr,0))) >= 0 THEN 'Dr' ELSE 'Cr' END AS BalanceType,
(case when @DivisionCnt>1 THEN '0' else max(DivisionId) end) as DivisionId,
(case when @SiteCnt>1 THEN '0' else max(siteId) end) as SiteId,
@SiteText as SiteText,@DivisionText as DivisionText,@FromDate as Fdate,@ToDate as ToDate,
'TrialBalanceSummery.rdl' AS ReportName, NULL AS SubReportProcList,'Detail Trial Balance' AS ReportTitle
FROM
(
SELECT LA.LedgerAccountGroupId,  sum(isnull(H.AmtDr,0)) - sum(isnull(H.AmtCr,0)) AS Opening,
0 AS AmtDr,0 AS  AmtCr,max(LH.SiteId) as siteId,max(LH.DivisionId) as DivisionId   
FROM web.Ledgers H WITH (Nolock)
LEFT JOIN web.LedgerHeaders LH WITH (Nolock) ON LH.LedgerHeaderId = H.LedgerHeaderId 
LEFT JOIN web.LedgerAccounts LA  WITH (Nolock) ON LA.LedgerAccountId = H.LedgerAccountId 
WHERE LA.LedgerAccountGroupId IS NOT NULL 
AND ( @Site is null or LH.SiteId IN (SELECT Items FROM [dbo].[Split] (@Site, ','))) 
AND ( @Division is null or LH.DivisionId IN (SELECT Items FROM [dbo].[Split] (@Division, ','))) 
AND ( @FromDate is null or @FromDate > LH.DocDate ) 
GROUP BY LA.LedgerAccountGroupId 
UNION ALL 

SELECT LA.LedgerAccountGroupId, 0 AS Opening,
CASE WHEN sum(isnull(H.AmtDr,0)) - sum(isnull(H.AmtCr,0)) > 0 THEN sum(isnull(H.AmtDr,0)) - sum(isnull(H.AmtCr,0)) ELSE 0 END AS AmtDr,
CASE WHEN sum(isnull(H.AmtDr,0)) - sum(isnull(H.AmtCr,0)) < 0 THEN abs(sum(isnull(H.AmtDr,0)) - sum(isnull(H.AmtCr,0))) ELSE 0 END AS AmtCr,   
Null as siteId,Null as DivisionId
FROM web.Ledgers H WITH (Nolock)
LEFT JOIN web.LedgerHeaders LH WITH (Nolock) ON LH.LedgerHeaderId = H.LedgerHeaderId 
LEFT JOIN web.LedgerAccounts LA  WITH (Nolock) ON LA.LedgerAccountId = H.LedgerAccountId 
LEFT JOIN web.LedgerAccountGroups LAG  WITH (Nolock) ON LAG.LedgerAccountGroupId = LA.LedgerAccountGroupId 
WHERE LA.LedgerAccountGroupId IS NOT NULL 
AND ( @Site is null or LH.SiteId IN (SELECT Items FROM [dbo].[Split] (@Site, ','))) 
AND ( @Division is null or LH.DivisionId IN (SELECT Items FROM [dbo].[Split] (@Division, ','))) 
AND ( @FromDate is null or @FromDate <= LH.DocDate ) 
AND ( @ToDate is null or @ToDate >= LH.DocDate ) 
GROUP BY LA.LedgerAccountGroupId 
) AS VMain
LEFT JOIN web.LedgerAccountGroups LAG  WITH (Nolock) ON LAG.LedgerAccountGroupId = VMain.LedgerAccountGroupId 
GROUP BY VMain.LedgerAccountGroupId
HAVING Sum(Isnull(VMain.Opening,0)) <> 0 OR Sum(isnull(Vmain.AmtDr,0)) <> 0 OR Sum(isnull(Vmain.AmtDr,0)) <> 0

End