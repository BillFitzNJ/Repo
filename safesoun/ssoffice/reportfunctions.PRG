
  FUNCTION l_storename
  PARAMETER pc_storecode
  ***************
  PRIVATE lc_alias, ;
  lc_order, ;
  ln_recno, ;
  lc_name, ;
  lc_exact, ;
  lc_storecode, ;
  ln_at
  lc_alias = ALIAS()
  lc_order = ORDER()
  ln_recno = IIF(EOF(), 0, RECNO())
  lc_name = ""
  lc_exact = SET("EXACT")
  lc_storecode = LEFT(pc_storecode,11)
  = SQLPREPARE(gnconnhandle, "SELECT * FROM store WHERE LEFT(storecode,11) = ?lc_storecode ", "curstoreseek")  
  ln_exec = SQLEXEC(gnconnhandle)
  IF ln_exec < 1
    WAIT WINDOW "SQL ERROR Has Occurred "
    = AERROR(la_error)
    WAIT WINDOW la_error(1,2)
    ll_cont = .F.
  ENDIF
  IF NOT EOF("curstoreseek")
    *-- If '-' at 3rd or 4th character, take all after for store name
    ln_at = AT("-", curstoreseek->store_name)
    IF ln_at = 3 OR ln_at = 4
      lc_name =  SUBSTR(curstoreseek->store_name, ln_at + 1)
    ELSE
      lc_name = curstoreseek->store_name
    ENDIF
  ELSE
    lc_name = "Unidentified Store"
  ENDIF
  SET EXACT &lc_exact
  IF NOT EMPTY(lc_alias)
    SELECT &lc_alias
  ENDIF
  IF NOT EMPTY(lc_order)
    SET ORDER TO &lc_order
  ENDIF
  IF ln_recno > 0
    GOTO ln_recno
  ENDIF
  RETURN lc_name

  FUNCTION lc_companybillname
  IF EMPTY(curbillcompany->billname)
    lc_billname = curbillcompany->name
  ELSE
    lc_billname = curbillcompany->billname
  ENDIF
  RETURN lc_billname




  ********************
  FUNCTION lf_ratecalc
  ********************
  IF NOT EMPTY(tax_rate)
    ln_printrate = tax_rate
  ENDIF
  RETURN ln_printrate

  *****************
  FUNCTION l_othamt
  *****************
  PRIVATE ln_charged
  ln_charged = 0

  DO CASE
  CASE trancode = "SL"
  CASE trancode = "CD"
  CASE trancode = "CN"
  CASE trancode = "ST"
  CASE trancode = "SU"
  CASE trancode = "DP"
  OTHERWISE
    ln_charged = charged
  ENDCASE

  RETURN ln_charged




