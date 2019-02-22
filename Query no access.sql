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