<<<<<<< .mine
IF object_id ('[Web].[spLedger]') IS NOT NULL 
 DROP Procedure  [Web].[spLedger]
GO 

CREATE PROCEDURE [Web].[spLedger]     
    @Site VARCHAR(20) = NULL,
    @Division VARCHAR(20) = NULL,
	@FromDate VARCHAR(20) = NULL,
	@ToDate VARCHAR(20) = NULL,
    @LedgerAccountId VARCHAR(20) = NULL
AS
BEGIN

/*
SELECT LH.DocNo, DT.DocumentTypeShortName, DT.DocumentTypeName, LH.DocDate, H.Narration,
H.LedgerId, H.AmtDr, H.AmtCr, 
sum(isnull(H.AmtDr,0)) OVER( PARTITION BY H.LedgerAccountId  ORDER BY LH.DocDate, LH.DocTypeId,  LH.DocNo, H.LedgerId   ) Sum_AmtDr,
sum(isnull(H.AmtCr,0)) OVER( PARTITION BY H.LedgerAccountId  ORDER BY LH.DocDate, LH.DocTypeId,  LH.DocNo,H.LedgerId ) Sum_AmtCr,
sum(isnull(H.AmtDr,0)) OVER( PARTITION BY H.LedgerAccountId  ORDER BY LH.DocDate, LH.DocTypeId,  LH.DocNo, H.LedgerId ) - sum(isnull(H.AmtCr,0)) OVER( PARTITION BY H.LedgerAccountId  ORDER BY LH.DocDate, LH.DocTypeId,  LH.DocNo ,H.LedgerId )  AS Balance,
CASE WHEN sum(isnull(H.AmtDr,0)) OVER( ORDER BY LH.DocDate, LH.DocTypeId,  LH.DocNo ,H.LedgerId ) - sum(isnull(H.AmtCr,0)) OVER( PARTITION BY H.LedgerAccountId  ORDER BY LH.DocDate, LH.DocTypeId,  LH.DocNo ,H.LedgerId )  >= 0 THEN 'Dr' ELSE 'Cr' END  AS BalanceType            
FROM web.Ledgers H  WITH (Nolock) 
LEFT JOIN web.LedgerHeaders LH WITH (Nolock) ON LH.LedgerHeaderId = H.LedgerHeaderId 
LEFT JOIN web.DocumentTypes DT WITH (Nolock) ON DT.DocumentTypeId = LH.DocTypeId 
LEFT JOIN web.LedgerAccounts LA  WITH (Nolock) ON LA.LedgerAccountId = H.LedgerAccountId 
ORDER BY LH.DocDate, LH.DocTypeId,  LH.DocNo,H.LedgerId
*/



SELECT VMain.LedgerAccountId, VMain.LedgerHeaderId, VMain.DocHeaderId, VMain.DocTypeId,  VMain.LedgerAccountName, VMain.ContraLedgerAccountName, 
VMain.DocNo, VMain.DocumentTypeShortName, REPLACE(CONVERT(VARCHAR(11),VMain.DocDate,106), ' ','/')   AS DocDate, VMain.Narration, VMain.LedgerId, 
CASE WHEN VMain.AmtDr = 0 THEN NULL ELSE VMain.AmtDr END AS AmtDr, CASE WHEN VMain.AmtCr = 0 THEN NULL ELSE VMain.AmtCr END AS AmtCr,  
abs(sum(isnull(VMain.AmtDr,0)) OVER( PARTITION BY VMain.LedgerAccountId  ORDER BY VMain.DocDate, VMain.DocTypeId,  VMain.DocNo, VMain.LedgerId ) - sum(isnull(VMain.AmtCr,0)) OVER( PARTITION BY VMain.LedgerAccountId  ORDER BY VMain.DocDate, VMain.DocTypeId,  VMain.DocNo ,VMain.LedgerId ))  AS Balance,
CASE WHEN sum(isnull(VMain.AmtDr,0)) OVER( ORDER BY VMain.DocDate, VMain.DocTypeId,  VMain.DocNo ,VMain.LedgerId ) - sum(isnull(VMain.AmtCr,0)) OVER( PARTITION BY VMain.LedgerAccountId  ORDER BY VMain.DocDate, VMain.DocTypeId,  VMain.DocNo ,VMain.LedgerId )  >= 0 THEN 'Dr' ELSE 'Cr' END  AS BalanceType 
FROM
(
SELECT H.LedgerAccountId, 0 AS LedgerHeaderId, 0 AS DocHeaderId, 0 AS DocTypeId, Max(LA.LedgerAccountName) AS LedgerAccountName,  'Opening' AS ContraLedgerAccountName, 'Opening' AS DocNo, 'Opening' AS DocumentTypeShortName, @FromDate AS DocDate, 'Opening' AS Narration,
0 AS LedgerId, 
CASE WHEN sum(isnull(H.AmtDr,0)) - sum(isnull(H.AmtCr,0)) > 0 THEN sum(isnull(H.AmtDr,0)) - sum(isnull(H.AmtCr,0)) ELSE 0 END AS AmtDr,
CASE WHEN sum(isnull(H.AmtDr,0)) - sum(isnull(H.AmtCr,0)) < 0 THEN abs(sum(isnull(H.AmtDr,0)) - sum(isnull(H.AmtCr,0))) ELSE 0 END AS AmtCr,
NULL AS DomainName, NULL AS ControllerActionId 
FROM web.Ledgers H  WITH (Nolock) 
LEFT JOIN web.LedgerHeaders LH WITH (Nolock) ON LH.LedgerHeaderId = H.LedgerHeaderId 
LEFT JOIN web.LedgerAccounts LA  WITH (Nolock) ON LA.LedgerAccountId = H.LedgerAccountId 
WHERE H.LedgerAccountId IS NOT NULL 
--AND ( @FromDate is null or @FromDate > LH.DocDate ) 
AND ( @FromDate > LH.DocDate ) 
AND ( @Site is null or LH.SiteId IN (SELECT Items FROM [dbo].[Split] (@Site, ','))) 
AND ( @Division is null or LH.DivisionId IN (SELECT Items FROM [dbo].[Split] (@Division, ','))) 
AND ( @LedgerAccountId is null or H.LedgerAccountId IN (SELECT Items FROM [dbo].[Split] (@LedgerAccountId, ','))) 
GROUP BY H.LedgerAccountId 

UNION ALL 

SELECT H.LedgerAccountId,  H.LedgerHeaderId, LH.DocHeaderId,  LH.DocTypeId,  LA.LedgerAccountName, CLA.LedgerAccountName AS ContraLedgerAccountName, LH.DocNo, DT.DocumentTypeShortName, LH.DocDate  AS DocDate, 
CASE WHEN isnull(H.Narration,'') = '' THEN CLA.LedgerAccountName ELSE CLA.LedgerAccountName + ' (' + H.Narration + ')' END AS Narration,
H.LedgerId, H.AmtDr, H.AmtCr, DT.DomainName, DT.ControllerActionId       
FROM web.Ledgers H  WITH (Nolock) 
LEFT JOIN web.LedgerHeaders LH WITH (Nolock) ON LH.LedgerHeaderId = H.LedgerHeaderId 
LEFT JOIN web.DocumentTypes DT WITH (Nolock) ON DT.DocumentTypeId = LH.DocTypeId 
LEFT JOIN web.LedgerAccounts LA  WITH (Nolock) ON LA.LedgerAccountId = H.LedgerAccountId 
LEFT JOIN web.LedgerAccounts CLA  WITH (Nolock) ON CLA.LedgerAccountId = H.ContraLedgerAccountId  
WHERE LA.LedgerAccountId IS NOT NULL 
AND ( @FromDate is null or @FromDate <= LH.DocDate ) 
AND ( @ToDate is null or @ToDate >= LH.DocDate ) 
AND ( @Site is null or LH.SiteId IN (SELECT Items FROM [dbo].[Split] (@Site, ','))) 
AND ( @Division is null or LH.DivisionId IN (SELECT Items FROM [dbo].[Split] (@Division, ','))) 
AND ( @LedgerAccountId is null or H.LedgerAccountId IN (SELECT Items FROM [dbo].[Split] (@LedgerAccountId, ','))) 
) VMain


End
GO
=======
��U S E   [ R U G ]  
 G O  
 / * * * * * *   O b j e c t :     S t o r e d P r o c e d u r e   [ W e b ] . [ s p L e d g e r ]         S c r i p t   D a t e :   0 5 / A u g / 2 0 1 5   1 2 : 1 2 : 2 2   P M   * * * * * * /  
 S E T   A N S I _ N U L L S   O N  
 G O  
 S E T   Q U O T E D _ I D E N T I F I E R   O N  
 G O  
 A L T E R   P R O C E D U R E   [ W e b ] . [ s p L e d g e r ]            
         @ S i t e   V A R C H A R ( 2 0 )   =   N U L L ,  
         @ D i v i s i o n   V A R C H A R ( 2 0 )   =   N U L L ,  
 	 @ F r o m D a t e   V A R C H A R ( 2 0 )   =   N U L L ,  
 	 @ T o D a t e   V A R C H A R ( 2 0 )   =   N U L L ,  
         @ L e d g e r A c c o u n t I d   V A R C H A R ( 2 0 )   =   N U L L  
 A S  
 B E G I N  
  
 d e c l a r e   @ D i v i s i o n C n t   I n t  
 d e c l a r e   @ S i t e C n t   I n t  
 s e t   @ D i v i s i o n C n t = ' 0 '  
 s e t   @ S i t e C n t = ' 0 '  
  
 D e c l a r e   @ S i t e T e x t   v a r c h a r ( m a x )  
 s e t   @ S i t e T e x t = ( S e l e c t   S i t e N a m e   +   ' , '   f r o m   W e b . S i t e s   w h e r e   S i t e I d   I n   ( S E L E C T   I t e m s   F R O M   [ d b o ] . [ S p l i t ]   ( @ S i t e ,   ' , ' ) )   F O R   X M L   p a t h   ( ' ' ) )  
 s e t   @ S i t e T e x t = L E F T ( @ S i t e T e x t , l e n ( @ S i t e T e x t ) - 1 )  
  
 d e c l a r e   @ D i v i s i o n T e x t   v a r c h a r ( m a x )  
 s e t   @ D i v i s i o n T e x t = ( S e l e c t   D i v i s i o n N a m e   +   ' , '   f r o m   w e b . D i v i s i o n s   w h e r e   D i v i s i o n I d   I n   ( S E L E C T   I t e m s   F R O M   [ d b o ] . [ S p l i t ]   ( @ D i v i s i o n ,   ' , ' ) )   F O R   X M L   p a t h   ( ' ' ) )  
 s e t   @ D i v i s i o n T e x t = L E F T ( @ D i v i s i o n T e x t , l e n ( @ D i v i s i o n T e x t ) - 1 )  
  
 D e c l a r e   @ L e d g e r A c c o u n t T e x t   v a r c h a r ( m a x )  
 S e t   @ L e d g e r A c c o u n t T e x t = ( S e l e c t   L e d g e r A c c o u n t N a m e   +   ' , '   f r o m   w e b . L e d g e r A c c o u n t s   w h e r e   L e d g e r A c c o u n t I d   I n   ( S E L E C T   I t e m s   F R O M   [ d b o ] . [ S p l i t ]   ( @ L e d g e r A c c o u n t I d ,   ' , ' ) )   F O R   X M L   p a t h   ( ' ' ) )  
 S e t   @ L e d g e r A c c o u n t T e x t = L E F T ( @ L e d g e r A c c o u n t T e x t , l e n ( @ L e d g e r A c c o u n t T e x t ) - 1 )  
  
  
  
 / *  
 S E L E C T   L H . D o c N o ,   D T . D o c u m e n t T y p e S h o r t N a m e ,   D T . D o c u m e n t T y p e N a m e ,   L H . D o c D a t e ,   H . N a r r a t i o n ,  
 H . L e d g e r I d ,   H . A m t D r ,   H . A m t C r ,    
 s u m ( i s n u l l ( H . A m t D r , 0 ) )   O V E R (   P A R T I T I O N   B Y   H . L e d g e r A c c o u n t I d     O R D E R   B Y   L H . D o c D a t e ,   L H . D o c T y p e I d ,     L H . D o c N o ,   H . L e d g e r I d       )   S u m _ A m t D r ,  
 s u m ( i s n u l l ( H . A m t C r , 0 ) )   O V E R (   P A R T I T I O N   B Y   H . L e d g e r A c c o u n t I d     O R D E R   B Y   L H . D o c D a t e ,   L H . D o c T y p e I d ,     L H . D o c N o , H . L e d g e r I d   )   S u m _ A m t C r ,  
 s u m ( i s n u l l ( H . A m t D r , 0 ) )   O V E R (   P A R T I T I O N   B Y   H . L e d g e r A c c o u n t I d     O R D E R   B Y   L H . D o c D a t e ,   L H . D o c T y p e I d ,     L H . D o c N o ,   H . L e d g e r I d   )   -   s u m ( i s n u l l ( H . A m t C r , 0 ) )   O V E R (   P A R T I T I O N   B Y   H . L e d g e r A c c o u n t I d     O R D E R   B Y   L H . D o c D a t e ,   L H . D o c T y p e I d ,     L H . D o c N o   , H . L e d g e r I d   )     A S   B a l a n c e ,  
 C A S E   W H E N   s u m ( i s n u l l ( H . A m t D r , 0 ) )   O V E R (   O R D E R   B Y   L H . D o c D a t e ,   L H . D o c T y p e I d ,     L H . D o c N o   , H . L e d g e r I d   )   -   s u m ( i s n u l l ( H . A m t C r , 0 ) )   O V E R (   P A R T I T I O N   B Y   H . L e d g e r A c c o u n t I d     O R D E R   B Y   L H . D o c D a t e ,   L H . D o c T y p e I d ,     L H . D o c N o   , H . L e d g e r I d   )     > =   0   T H E N   ' D r '   E L S E   ' C r '   E N D     A S   B a l a n c e T y p e                          
 F R O M   w e b . L e d g e r s   H     W I T H   ( N o l o c k )    
 L E F T   J O I N   w e b . L e d g e r H e a d e r s   L H   W I T H   ( N o l o c k )   O N   L H . L e d g e r H e a d e r I d   =   H . L e d g e r H e a d e r I d    
 L E F T   J O I N   w e b . D o c u m e n t T y p e s   D T   W I T H   ( N o l o c k )   O N   D T . D o c u m e n t T y p e I d   =   L H . D o c T y p e I d    
 L E F T   J O I N   w e b . L e d g e r A c c o u n t s   L A     W I T H   ( N o l o c k )   O N   L A . L e d g e r A c c o u n t I d   =   H . L e d g e r A c c o u n t I d    
 O R D E R   B Y   L H . D o c D a t e ,   L H . D o c T y p e I d ,     L H . D o c N o , H . L e d g e r I d  
 * /  
  
  
  
  
 / *   f o r   t h e   D i v i s i o n   a n d   D i v i s i o n   C o u n t   * /  
 S E L E C T   @ D i v i s i o n C n t = c o u n t ( D i s t i n c t   V M a i n 1 . D i v i s i o n I d ) , @ S i t e C n t = c o u n t ( D i s t i n c t   V M a i n 1 . S i t e I d )  
 F R O M  
 (  
 S E L E C T   H . L e d g e r A c c o u n t I d ,   0   A S   L e d g e r H e a d e r I d ,   0   A S   D o c H e a d e r I d ,   0   A S   D o c T y p e I d ,   M a x ( L A . L e d g e r A c c o u n t N a m e )   A S   L e d g e r A c c o u n t N a m e ,     ' O p e n i n g '   A S   C o n t r a L e d g e r A c c o u n t N a m e ,   ' O p e n i n g '   A S   D o c N o ,   ' O p e n i n g '   A S   D o c u m e n t T y p e S h o r t N a m e ,   @ F r o m D a t e   A S   D o c D a t e ,   ' O p e n i n g '   A S   N a r r a t i o n ,  
 0   A S   L e d g e r I d ,    
 C A S E   W H E N   s u m ( i s n u l l ( H . A m t D r , 0 ) )   -   s u m ( i s n u l l ( H . A m t C r , 0 ) )   >   0   T H E N   s u m ( i s n u l l ( H . A m t D r , 0 ) )   -   s u m ( i s n u l l ( H . A m t C r , 0 ) )   E L S E   0   E N D   A S   A m t D r ,  
 C A S E   W H E N   s u m ( i s n u l l ( H . A m t D r , 0 ) )   -   s u m ( i s n u l l ( H . A m t C r , 0 ) )   <   0   T H E N   a b s ( s u m ( i s n u l l ( H . A m t D r , 0 ) )   -   s u m ( i s n u l l ( H . A m t C r , 0 ) ) )   E L S E   0   E N D   A S   A m t C r ,  
 N U L L   A S   D o m a i n N a m e ,   N U L L   A S   C o n t r o l l e r A c t i o n I d , m a x ( L H . S i t e I d )   a s   S i t e I d , m a x ( L H . D i v i s i o n I d )   a s   D i v i s i o n I d  
 F R O M   w e b . L e d g e r s   H     W I T H   ( N o l o c k )    
 L E F T   J O I N   w e b . L e d g e r H e a d e r s   L H   W I T H   ( N o l o c k )   O N   L H . L e d g e r H e a d e r I d   =   H . L e d g e r H e a d e r I d    
 L E F T   J O I N   w e b . L e d g e r A c c o u n t s   L A     W I T H   ( N o l o c k )   O N   L A . L e d g e r A c c o u n t I d   =   H . L e d g e r A c c o u n t I d    
 W H E R E   H . L e d g e r A c c o u n t I d   I S   N O T   N U L L    
 - - A N D   (   @ F r o m D a t e   i s   n u l l   o r   @ F r o m D a t e   >   L H . D o c D a t e   )    
 A N D   (   @ F r o m D a t e   >   L H . D o c D a t e   )    
 A N D   (   @ S i t e   i s   n u l l   o r   L H . S i t e I d   I N   ( S E L E C T   I t e m s   F R O M   [ d b o ] . [ S p l i t ]   ( @ S i t e ,   ' , ' ) ) )    
 A N D   (   @ D i v i s i o n   i s   n u l l   o r   L H . D i v i s i o n I d   I N   ( S E L E C T   I t e m s   F R O M   [ d b o ] . [ S p l i t ]   ( @ D i v i s i o n ,   ' , ' ) ) )    
 A N D   (   @ L e d g e r A c c o u n t I d   i s   n u l l   o r   H . L e d g e r A c c o u n t I d   I N   ( S E L E C T   I t e m s   F R O M   [ d b o ] . [ S p l i t ]   ( @ L e d g e r A c c o u n t I d ,   ' , ' ) ) )    
 G R O U P   B Y   H . L e d g e r A c c o u n t I d    
  
 U N I O N   A L L    
  
 S E L E C T   H . L e d g e r A c c o u n t I d ,     H . L e d g e r H e a d e r I d ,   L H . D o c H e a d e r I d ,     L H . D o c T y p e I d ,     L A . L e d g e r A c c o u n t N a m e ,   C L A . L e d g e r A c c o u n t N a m e   A S   C o n t r a L e d g e r A c c o u n t N a m e ,   L H . D o c N o ,   D T . D o c u m e n t T y p e S h o r t N a m e ,   L H . D o c D a t e     A S   D o c D a t e ,    
 C A S E   W H E N   i s n u l l ( H . N a r r a t i o n , ' ' )   =   ' '   T H E N   C L A . L e d g e r A c c o u n t N a m e   E L S E   C L A . L e d g e r A c c o u n t N a m e   +   '   ( '   +   H . N a r r a t i o n   +   ' ) '   E N D   A S   N a r r a t i o n ,  
 H . L e d g e r I d ,   H . A m t D r ,   H . A m t C r ,   D T . D o m a i n N a m e ,   D T . C o n t r o l l e r A c t i o n I d , L H . S i t e I d , L H . D i v i s i o n I d  
 F R O M   w e b . L e d g e r s   H     W I T H   ( N o l o c k )    
 L E F T   J O I N   w e b . L e d g e r H e a d e r s   L H   W I T H   ( N o l o c k )   O N   L H . L e d g e r H e a d e r I d   =   H . L e d g e r H e a d e r I d    
 L E F T   J O I N   w e b . D o c u m e n t T y p e s   D T   W I T H   ( N o l o c k )   O N   D T . D o c u m e n t T y p e I d   =   L H . D o c T y p e I d    
 L E F T   J O I N   w e b . L e d g e r A c c o u n t s   L A     W I T H   ( N o l o c k )   O N   L A . L e d g e r A c c o u n t I d   =   H . L e d g e r A c c o u n t I d    
 L E F T   J O I N   w e b . L e d g e r A c c o u n t s   C L A     W I T H   ( N o l o c k )   O N   C L A . L e d g e r A c c o u n t I d   =   H . C o n t r a L e d g e r A c c o u n t I d      
 W H E R E   L A . L e d g e r A c c o u n t I d   I S   N O T   N U L L    
 A N D   (   @ F r o m D a t e   i s   n u l l   o r   @ F r o m D a t e   < =   L H . D o c D a t e   )    
 A N D   (   @ T o D a t e   i s   n u l l   o r   @ T o D a t e   > =   L H . D o c D a t e   )    
 A N D   (   @ S i t e   i s   n u l l   o r   L H . S i t e I d   I N   ( S E L E C T   I t e m s   F R O M   [ d b o ] . [ S p l i t ]   ( @ S i t e ,   ' , ' ) ) )    
 A N D   (   @ D i v i s i o n   i s   n u l l   o r   L H . D i v i s i o n I d   I N   ( S E L E C T   I t e m s   F R O M   [ d b o ] . [ S p l i t ]   ( @ D i v i s i o n ,   ' , ' ) ) )    
 A N D   (   @ L e d g e r A c c o u n t I d   i s   n u l l   o r   H . L e d g e r A c c o u n t I d   I N   ( S E L E C T   I t e m s   F R O M   [ d b o ] . [ S p l i t ]   ( @ L e d g e r A c c o u n t I d ,   ' , ' ) ) )    
 )   V M a i n 1  
  
 / *   m a i n   Q u e r y   * /  
  
 S E L E C T   V M a i n . L e d g e r A c c o u n t I d ,   V M a i n . L e d g e r H e a d e r I d ,   V M a i n . D o c H e a d e r I d ,   V M a i n . D o c T y p e I d ,     V M a i n . L e d g e r A c c o u n t N a m e   a s   R e p o r t T i t l e ,   V M a i n . C o n t r a L e d g e r A c c o u n t N a m e ,    
 V M a i n . D o c N o ,   V M a i n . D o c u m e n t T y p e S h o r t N a m e ,   R E P L A C E ( C O N V E R T ( V A R C H A R ( 1 1 ) , V M a i n . D o c D a t e , 1 0 6 ) ,   '   ' , ' / ' )       A S   D o c D a t e ,   V M a i n . N a r r a t i o n ,   V M a i n . L e d g e r I d ,    
 C A S E   W H E N   V M a i n . A m t D r   =   0   T H E N   N U L L   E L S E   V M a i n . A m t D r   E N D   A S   A m t D r ,   C A S E   W H E N   V M a i n . A m t C r   =   0   T H E N   N U L L   E L S E   V M a i n . A m t C r   E N D   A S   A m t C r ,      
 a b s ( s u m ( i s n u l l ( V M a i n . A m t D r , 0 ) )   O V E R (   P A R T I T I O N   B Y   V M a i n . L e d g e r A c c o u n t I d     O R D E R   B Y   V M a i n . D o c D a t e ,   V M a i n . D o c T y p e I d ,     V M a i n . D o c N o ,   V M a i n . L e d g e r I d   )   -   s u m ( i s n u l l ( V M a i n . A m t C r , 0 ) )   O V E R (   P A R T I T I O N   B Y   V M a i n . L e d g e r A c c o u n t I d     O R D E R   B Y   V M a i n . D o c D a t e ,   V M a i n . D o c T y p e I d ,     V M a i n . D o c N o   , V M a i n . L e d g e r I d   ) )     A S   B a l a n c e ,  
 C A S E   W H E N   s u m ( i s n u l l ( V M a i n . A m t D r , 0 ) )   O V E R (   O R D E R   B Y   V M a i n . D o c D a t e ,   V M a i n . D o c T y p e I d ,     V M a i n . D o c N o   , V M a i n . L e d g e r I d   )   -   s u m ( i s n u l l ( V M a i n . A m t C r , 0 ) )   O V E R (   P A R T I T I O N   B Y   V M a i n . L e d g e r A c c o u n t I d     O R D E R   B Y   V M a i n . D o c D a t e ,   V M a i n . D o c T y p e I d ,     V M a i n . D o c N o   , V M a i n . L e d g e r I d   )     > =   0   T H E N   ' D r '   E L S E   ' C r '   E N D     A S   B a l a n c e T y p e ,  
 ' L e d g e r . r d l '   A S   R e p o r t N a m e ,   N U L L   A S   S u b R e p o r t P r o c L i s t , ( c a s e   w h e n   @ S i t e C n t > 1   T H E N   ' 0 '   e l s e   V M a i n . S i t e I d   e n d )   a s   S i t e I d , ( c a s e   w h e n   @ D i v i s i o n C n t > 1   T H E N   ' 0 '   e l s e   V M a i n . D i v i s i o n I d   e n d )   a s   D i v i s i o n I d             ,  
 @ S i t e T e x t   a s   S i t e T e x t , @ D i v i s i o n T e x t   a s   D i v i s i o n T e x t , @ L e d g e r A c c o u n t T e x t   a s   L e d g e r A c c o u n t T e x t , @ F r o m D a t e   a s   F r o m D a t e , @ T o D a t e   a s   T o D a t e  
 F R O M  
 (  
 S E L E C T   H . L e d g e r A c c o u n t I d ,   0   A S   L e d g e r H e a d e r I d ,   0   A S   D o c H e a d e r I d ,   0   A S   D o c T y p e I d ,   M a x ( L A . L e d g e r A c c o u n t N a m e )   A S   L e d g e r A c c o u n t N a m e ,     ' O p e n i n g '   A S   C o n t r a L e d g e r A c c o u n t N a m e ,   ' O p e n i n g '   A S   D o c N o ,   ' O p e n i n g '   A S   D o c u m e n t T y p e S h o r t N a m e ,   @ F r o m D a t e   A S   D o c D a t e ,   ' O p e n i n g '   A S   N a r r a t i o n ,  
 0   A S   L e d g e r I d ,    
 C A S E   W H E N   s u m ( i s n u l l ( H . A m t D r , 0 ) )   -   s u m ( i s n u l l ( H . A m t C r , 0 ) )   >   0   T H E N   s u m ( i s n u l l ( H . A m t D r , 0 ) )   -   s u m ( i s n u l l ( H . A m t C r , 0 ) )   E L S E   0   E N D   A S   A m t D r ,  
 C A S E   W H E N   s u m ( i s n u l l ( H . A m t D r , 0 ) )   -   s u m ( i s n u l l ( H . A m t C r , 0 ) )   <   0   T H E N   a b s ( s u m ( i s n u l l ( H . A m t D r , 0 ) )   -   s u m ( i s n u l l ( H . A m t C r , 0 ) ) )   E L S E   0   E N D   A S   A m t C r ,  
 N U L L   A S   D o m a i n N a m e ,   N U L L   A S   C o n t r o l l e r A c t i o n I d , m a x ( L H . S i t e I d )   a s   S i t e I d , m a x ( L H . D i v i s i o n I d )   a s   D i v i s i o n I d  
 F R O M   w e b . L e d g e r s   H     W I T H   ( N o l o c k )    
 L E F T   J O I N   w e b . L e d g e r H e a d e r s   L H   W I T H   ( N o l o c k )   O N   L H . L e d g e r H e a d e r I d   =   H . L e d g e r H e a d e r I d    
 L E F T   J O I N   w e b . L e d g e r A c c o u n t s   L A     W I T H   ( N o l o c k )   O N   L A . L e d g e r A c c o u n t I d   =   H . L e d g e r A c c o u n t I d    
 W H E R E   H . L e d g e r A c c o u n t I d   I S   N O T   N U L L    
 - - A N D   (   @ F r o m D a t e   i s   n u l l   o r   @ F r o m D a t e   >   L H . D o c D a t e   )    
 A N D   (   @ F r o m D a t e   >   L H . D o c D a t e   )    
 A N D   (   @ S i t e   i s   n u l l   o r   L H . S i t e I d   I N   ( S E L E C T   I t e m s   F R O M   [ d b o ] . [ S p l i t ]   ( @ S i t e ,   ' , ' ) ) )    
 A N D   (   @ D i v i s i o n   i s   n u l l   o r   L H . D i v i s i o n I d   I N   ( S E L E C T   I t e m s   F R O M   [ d b o ] . [ S p l i t ]   ( @ D i v i s i o n ,   ' , ' ) ) )    
 A N D   (   @ L e d g e r A c c o u n t I d   i s   n u l l   o r   H . L e d g e r A c c o u n t I d   I N   ( S E L E C T   I t e m s   F R O M   [ d b o ] . [ S p l i t ]   ( @ L e d g e r A c c o u n t I d ,   ' , ' ) ) )    
 G R O U P   B Y   H . L e d g e r A c c o u n t I d    
  
 U N I O N   A L L    
  
 S E L E C T   H . L e d g e r A c c o u n t I d ,     H . L e d g e r H e a d e r I d ,   L H . D o c H e a d e r I d ,     L H . D o c T y p e I d ,     L A . L e d g e r A c c o u n t N a m e ,   C L A . L e d g e r A c c o u n t N a m e   A S   C o n t r a L e d g e r A c c o u n t N a m e ,   L H . D o c N o ,   D T . D o c u m e n t T y p e S h o r t N a m e ,   L H . D o c D a t e     A S   D o c D a t e ,    
 C A S E   W H E N   i s n u l l ( H . N a r r a t i o n , ' ' )   =   ' '   T H E N   C L A . L e d g e r A c c o u n t N a m e   E L S E   C L A . L e d g e r A c c o u n t N a m e   +   '   ( '   +   H . N a r r a t i o n   +   ' ) '   E N D   A S   N a r r a t i o n ,  
 H . L e d g e r I d ,   H . A m t D r ,   H . A m t C r ,   D T . D o m a i n N a m e ,   D T . C o n t r o l l e r A c t i o n I d , L H . S i t e I d , L H . D i v i s i o n I d              
 F R O M   w e b . L e d g e r s   H     W I T H   ( N o l o c k )    
 L E F T   J O I N   w e b . L e d g e r H e a d e r s   L H   W I T H   ( N o l o c k )   O N   L H . L e d g e r H e a d e r I d   =   H . L e d g e r H e a d e r I d    
 L E F T   J O I N   w e b . D o c u m e n t T y p e s   D T   W I T H   ( N o l o c k )   O N   D T . D o c u m e n t T y p e I d   =   L H . D o c T y p e I d    
 L E F T   J O I N   w e b . L e d g e r A c c o u n t s   L A     W I T H   ( N o l o c k )   O N   L A . L e d g e r A c c o u n t I d   =   H . L e d g e r A c c o u n t I d    
 L E F T   J O I N   w e b . L e d g e r A c c o u n t s   C L A     W I T H   ( N o l o c k )   O N   C L A . L e d g e r A c c o u n t I d   =   H . C o n t r a L e d g e r A c c o u n t I d      
 W H E R E   L A . L e d g e r A c c o u n t I d   I S   N O T   N U L L    
 A N D   (   @ F r o m D a t e   i s   n u l l   o r   @ F r o m D a t e   < =   L H . D o c D a t e   )    
 A N D   (   @ T o D a t e   i s   n u l l   o r   @ T o D a t e   > =   L H . D o c D a t e   )    
 A N D   (   @ S i t e   i s   n u l l   o r   L H . S i t e I d   I N   ( S E L E C T   I t e m s   F R O M   [ d b o ] . [ S p l i t ]   ( @ S i t e ,   ' , ' ) ) )    
 A N D   (   @ D i v i s i o n   i s   n u l l   o r   L H . D i v i s i o n I d   I N   ( S E L E C T   I t e m s   F R O M   [ d b o ] . [ S p l i t ]   ( @ D i v i s i o n ,   ' , ' ) ) )    
 A N D   (   @ L e d g e r A c c o u n t I d   i s   n u l l   o r   H . L e d g e r A c c o u n t I d   I N   ( S E L E C T   I t e m s   F R O M   [ d b o ] . [ S p l i t ]   ( @ L e d g e r A c c o u n t I d ,   ' , ' ) ) )    
 )   V M a i n  
 E n d  
  
  
  
  
  
  
  
 >>>>>>> .r721
