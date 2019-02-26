SELECT *
FROM TBL_KIT
WHERE (((TBL_KIT.[kit_cod_item]) = '2003747') AND (TBL_KIT.[SCM]) ALike 'cs_val_esf_entrada_lat_tql') AND ((TBL_KIT.[KIT_COD]) In (SELECT tbl_kit.kit_cod
FROM tbl_kit
WHERE (((tbl_kit.kit_cod_item)="2047214"))
));

--Seleciona todos os racks que tem TQL de 150 litros "2047214", depois os racks que tem válvulas de 3 1/8" na localização 's_val_esf_entrada_lat_tql'



SELECT DISTINCT TBL_KIT.KIT_COD_ITEM AS Expr1
FROM TBL_KIT
WHERE (((TBL_KIT.[SCM]) ALike 'cs_val_esf_entrada_lat_tql') AND ((TBL_KIT.[KIT_COD]) In (SELECT tbl_kit.kit_cod
FROM tbl_kit
WHERE (((tbl_kit.kit_cod_item)="2047214"))
)));

--Seleciona todos os racks com o TQL "2047214", depois todos os codigos da linha que tem o "SCM" 'cs_val_esf_entrada_lat_tql'.
--Retorna os códigos das válvulas da entrada do TQL, para descobrir as bitólas de entrada.


INSERT INTO tbl_kit ( kit_cod, kit_cod_item, qt, sequencia, scm, descricao )
SELECT "4020141FULL" AS ["kit_cod"], tbl_kit.kit_cod_item, tbl_kit.qt, tbl_kit.sequencia, tbl_kit.scm, tbl_kit.descricao
FROM tbl_kit
WHERE (((tbl_kit.[kit_cod]) ALike "4020141"));


--Usar para criar um rack novo usando um existente como base.
--Do rack 4020001 foi criado o 4020129. Antes de realizar o insert, criar o item 4020129 na tbl_item para não dar problema na integridade referencial.



SELECT * INTO SAIDATQL150L158ELEMENTO2
FROM TBL_KIT
WHERE (((TBL_KIT.SCM) ALike 'cs_carcaca_liq' Or (TBL_KIT.SCM) ALike 'cs_carcaca_liq_2elementos') AND ((TBL_KIT.kit_cod_item)='2003695' Or (TBL_KIT.kit_cod_item)='2003697') AND ((TBL_KIT.KIT_COD) In (SELECT tbl_kit.kit_cod
FROM tbl_kit
WHERE (((tbl_kit.kit_cod_item)="2047214"))
)));
--Seleciona o TQL depois as carcaças que tem a mesma bitola.
--2003695	"CARCACA 1 5/8"" DCR 2 ELEMENTO"
--2003697	"CARCACA 1 5/8"" DCR 3 ELEMENTO"
--2047214	TQL 150 VERT. STD ROT PT 3 1/8


SELECT tbl_kit.kit_cod, tbl_kit.kit_cod_item
FROM tbl_kit
WHERE tbl_kit.SCM ='cs_s_pur_ramal_entrada_maq' AND TBL_KIT.KIT_COD IN (
SELECT tbl_kit.kit_cod
FROM tbl_Item INNER JOIN (tbl_kit INNER JOIN tbl_Item AS tbl_Item_1 ON tbl_kit.kit_cod_item = tbl_Item_1.item_cod) ON tbl_Item.item_cod = tbl_kit.kit_cod
WHERE tbl_Item_1.item_desc ALike 'COL S% %1 3/8"'
ORDER BY tbl_kit.kit_cod, tbl_kit.sequencia)
--


UPDATE tbl_kit SET tbl_kit.kit_cod_item = "3002931"
WHERE (((tbl_kit.kit_cod) In (SELECT tbl_kit.kit_cod
FROM tbl_Item INNER JOIN (tbl_kit INNER JOIN tbl_Item AS tbl_Item_1 ON tbl_kit.kit_cod_item = tbl_Item_1.item_cod) ON tbl_Item.item_cod = tbl_kit.kit_cod
WHERE tbl_Item_1.item_desc ALike 'COL S% %3 1/8"'
ORDER BY tbl_kit.kit_cod, tbl_kit.sequencia)) AND ((tbl_kit.SCM) ALike 'cs_s_pur_ramal_entrada_maq'));
--