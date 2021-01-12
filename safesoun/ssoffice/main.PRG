*********
*MAIN.PRG
*********
PARAMETER pc_user
CLOSE ALL
CLEAR ALL
pc_user = "SRS"
#DEFINE cn_htscrfont  FONTMETRIC(1,"FoxFont",9) + FONTMETRIC(5,"FoxFont",9)
#DEFINE cn_wdscrfont  FONTMETRIC(6,"FoxFont",9)
#DEFINE cn_minwrows   480/cn_htscrfont     && Min rows - WINDOWS
#DEFINE cn_minwcols   640/cn_wdscrfont     && Min cols - WINDOWS
#DEFINE cc_white         255,255,255
#DEFINE cc_black         0,0,0
#DEFINE cc_black_1       1,1,1
#DEFINE cc_gray          192,192,192
#DEFINE cc_darkgray      128,128,128
#DEFINE cc_green         0,255,0
#DEFINE cc_darkgreen     0,128,0
#DEFINE cc_cyan          0,255,255
#DEFINE cc_darkcyan      0,128,128
#DEFINE cc_blue          0,0,255
#DEFINE cc_darkblue      0,0,128
#DEFINE cc_magenta       255,0,255
#DEFINE cc_darkmagenta   128,0,128
CLEAR
PUBLIC  gc_compname,;
  gc_reporthead,;
  gc_compad1,;
  gc_compad2,;
  gc_compstate,;
  gc_compcity,;
  gc_compzip,;
  gc_compphone, ;
  gc_recattn,;
  gc_recatt2,;
  gc_printcom,;
  gc_compid,;
  gd_sdate, ;
  gc_sqlserver, ;
  gc_sqldbname,;
  gf_sysdataf, ;
  gf_armastf, ;
  gf_arcustf, ;
  gf_arcashf, ;
  gf_acrecgf, ;
  gf_sycrlstf, ;
  gc_sqluser, ;
  gf_sydindxf,;
  gc_sqlpasswd, ;
  gf_acschdf
PUBLIC ld_bdate, ld_edate
PUBLIC lc_datapath
gc_compid = "01"

PUBLIC  gnConnHandle
gnConnHandle = -1

PRIVATE ll_abort, ;
  lc_type
ll_abort = .f.
lc_type = "S"

SET BELL OFF
SET ESCAPE OFF
SET ECHO OFF
SET EXCLUSIVE OFF
SET CENTURY OFF
SET SAFETY OFF
SET STRICTDATE TO 0
SET STATUS OFF
SET MESSAGE TO SPACE(60)
SET TALK OFF
SET DELETED ON
PUSH KEY CLEAR

INSMODE(.T.)
CLOSE DATABASES
SET KEYCOMP TO WINDOWS
RELEASE WINDOW STANDARD
* -- Load System Procedure.
SET PROCEDURE TO main


*-- Establish system date
gd_sdate = DATE()
*-- Establish class libraries
SET CLASSLIB TO ssofficelib
SET CLASSLIB TO ssofficeapplib ADDITIVE


*-- Instantiate application objects
oApp = CREATEOBJECT("app")




*-- Establish variables for in house testing.
ll_srs = .F.

*-- Open compdata and get locations for tables
IF NOT l_setcomp()
  ll_abort = .T.
ENDIF
IF NOT ll_abort
  *-- Connect to SQL Server

  gc_sqlserver =  ALLTRIM(compdata->sqlserver)
  gc_dbname = ALLTRIM(compdata->sqldbname)
  gc_sqluser =  ALLTRIM(compdata->sqluser)
  gc_sqlpassword =  ALLTRIM(compdata->sqlpasswd)

  lcConnString ="Driver=SQL Server;Server=" + gc_sqlserver  + ";Database=" + gc_dbname
  lcConnString = lcConnString + ";UID=" + gc_sqluser + ";PWD=" + gc_sqlpassword
  oApp.lc_pdfpath = ALLTRIM(compdata->pdfpath)
   
  gnConnHandle = SQLStringConnect(lcConnString)


  IF gnConnHandle > 0
    = SQLSETPROP(gnConnHandle, 'Transactions', 2)  && Manual transactions
    = SQLSETPROP(gnConnHandle, 'asynchronous', .F.)
  ELSE
    WAIT WINDOW "Unable to make SQL Connection"
    ll_abort = .T.
  ENDIF

  IF gnConnHandle > 0
    = SQLSETPROP(gnConnHandle, 'Transactions', 2)  && Manual transactions
    = SQLSETPROP(gnConnHandle, 'asynchronous', .F.)
    IF UPPER(ALLTRIM(GETENV("SBTUSERID"))) = "WSG"
      oApp.gc_user = "JKEL"
    ELSE
      *-- Login before entering the system
      oLogin = CREATEOBJECT("frmlogin")
      oLogin.Show()
    ENDIF
  ELSE
    DO notice WITH "Unable to make SQL Connection"
    ll_abort = .T.
  ENDIF
ENDIF NOT ll_abort
IF NOT ll_abort


  * -- Establish Error Procedure
  ON ERROR DO syserror IN main WITH ERROR(), MESSAGE(), MESSAGE(11)
  SET TYPEAHEAD TO 20

  *-- Get operating system
  mosystem = OS()

  ll_cont = .T.

  *-- Establish testing mode
  IF TYPE("pc_user") = "C"
    gc_enduser = pc_user
  ELSE

    *-- Get system variable "enduser".
    IF UPPER(ALLTRIM(GETENV("enduser"))) = "SRS"
      gc_enduser = "SRS"
    ELSE
      gc_enduser = UPPER(ALLTRIM(RIGHT(SYS(0), LEN(SYS(0))- AT("#", SYS(0)))))
    ENDIF

  ENDIF
  *-- Return to OS if variable not set up.
  IF EMPTY(gc_enduser)
    DO notice WITH "Variable 'enduser' not established." + CHR(13) +;
      "Check with your System Administrator." + CHR(13) +;
      "Returning to Operating System."
    ll_cont = .F.
  ENDIF

  *-- Establish variables for in house testing.

  ll_srs = .F.

  IF ll_cont
    *-- Allow debugging for SRS.
    IF gc_enduser = "SRS" OR UPPER(ALLTRIM(GETENV("enduser"))) = "SRS"
      ll_srs = .T.
      ON KEY LABEL f1 SUSPEND
      ON KEY LABEL ctrl+f9 DO lf_reset IN main
    ENDIF

    *-- Open all tables for shared use
    ll_cont    = lf_allopen()

  ENDIF

  IF ll_cont

    *-- Scheme 1: User windows.
    SET COLOR OF SCHEME 1 TO ;
      RGB(cc_black,cc_gray),  RGB(cc_black,cc_white), ;
      RGB(cc_black,cc_white), RGB(cc_black,cc_white), ;
      RGB(cc_black,cc_gray),, ;
      RGB(cc_black,cc_white), RGB(cc_black,cc_white), ;
      RGB(cc_black_1,cc_gray), RGB(cc_black,cc_white)


    MODIFY WINDOW SCREEN ;
      TITLE "Safe and Sound Armed Courier" ;
      FONT "Courier New", 9 ;
      NOCLOSE;
      ICON FILE "WSGMOUSE.ICO" ;
      COLOR RGB(cc_black,cc_white), RGB(cc_black,cc_gray)

    IF SYSMETRIC(1) > 640 .AND. SYSMETRIC(2) > 480
      MOVE WINDOW SCREEN CENTER
    ELSE
      ZOOM WINDOW SCREEN MAX
    ENDIF
    MOVE WINDOW SCREEN CENTER
    ZOOM WINDOW SCREEN MAX

    Activate SCREEN

    *-- The FILL TO needs a CLEAR to function reliably
    Clear
    @ 0, 0 ;
      FILL TO SROWS(), SCOLS() ;
      COLOR RGB(255,255,255,255,255,255)


    *-- Standard Windows font for all browse screens.
    gc_brfnt    = ["MS SANS SERIF", 9]

    DO l_menuproc
  ENDIF && ll_cont

ELSE
  DO notice WITH "Login Cancelled"
ENDIF  && Enter login and password

*-- Cleanup
CLEAR EVENTS
SET SYSMENU TO DEFAULT
SET SYSMENU ON
SET PROCEDURE TO
CLOSE ALL DATABASES

IF ll_srs
  = SQLDISCONNECT(gnConnHandle)
  CLEAR ALL WINDOWS
  CLEAR
  RETURN
ELSE
  = SQLDISCONNECT(gnConnHandle)
  CLEAR ALL WINDOWS
  CLEAR
  QUIT
ENDIF




******************
PROCEDURE syserror
******************
PARAMETER errornum, ERRORMES, errorcalledby
PRIVATE ln_response, ;
  lc_errmess, la_error ;
  ln_type
CLEAR TYPEAHEAD
STORE 0 TO mln

lc_errmess = ERRORMES
ln_type = 18
IF errornum = 1429
  = AERROR(la_error)
  lc_errmess = la_error(1,2)
  ln_type = 0
ENDIF

ln_response = MESSAGEBOX(ERRORMES,18,"Error Discovered")


DO CASE
CASE errornum = 1429
  RETURN
CASE ln_response = 5
  RETURN
OTHERWISE
  TRY
    SUSPEND
  CATCH
    CLOSE ALL
    QUIT
  ENDTRY
ENDCASE

RETURN

****************
PROCEDURE notice
****************
* The message passed with mnotice will be displayed
PARAMETER mnotice

= MESSAGEBOX(mnotice,64,"Notice")

RETURN

*******************
PROCEDURE lf_reset
*******************
SET SYSMENU TO DEFAULT
SET SYSMENU ON
= INSMODE(.T.)
SUSPEND
RETURN

***************
PROCEDURE reply
***************
PARAMETER pc_message

PRIVATE lc_reply , ln_reply
lc_reply = "N"
ln_reply = MESSAGEBOX(pc_message,36,"Please Reply")
DO CASE
CASE ln_reply = 6
  lc_reply = "Y"
ENDCASE
RETURN lc_reply

*********************
FUNCTION lf_reportdef
*********************
*-- To call reports in compiler
IF 1 <> 1
ENDIF
RETURN

**********************
FUNCTION lf_juliandate
**********************
PARAMETER ld_date
PRIVATE ln_date, lc_date
ln_date =ld_date - CTOD("12/31/" + RIGHT(STR(YEAR(ld_date)-1),4))
lc_date = RIGHT(STR(10000 + ln_date),3)
RETURN lc_date


****************
FUNCTION g_fmttm
****************
PARAMETER pc_time
RETURN LEFT(pc_time,5)

****************
FUNCTION g_fmthd
****************
RETURN lc_head

**************
PROCEDURE l_menuproc
**************
* -- Main Menu Repeat Until Exit

DO WHILE .T.
  ll_menexit = .F.
  *-- Call main menu.
  ln_mencol = 0
  ln_menrow = 0
  lc_parm = ""

  oApp.gf_setsysmessage()

  DO ssofficemen.mpr
  *-  Activate the menu
  KEYBOARD "{F10}T"
  READ EVENTS
  DO CASE
    ******
    * FILE
    ******
  CASE ln_mencol = 1
    DO CASE
    CASE ln_menrow = 1
      EXIT
    ENDCASE
    **************
    * TRANSACTIONS
    **************
  CASE ln_mencol = 2
    DO CASE
    CASE ln_menrow = 1
     DO notice WITH "Use new system"
     **  oApp.lf_generatebills
    CASE ln_menrow = 2
      oApp.lf_deleteslips
    CASE ln_menrow = 3
      oApp.lf_applypayments("Cash")
    CASE ln_menrow = 4
      oApp.lf_createinvoice
    CASE ln_menrow = 5
      oApp.lf_applycredits
    CASE ln_menrow = 6
      oApp.lf_changeinvoice
    CASE ln_menrow = 7
      oApp.lf_postbills
    CASE ln_menrow = 8
      oApp.lf_applypayments("ACH")
    CASE ln_menrow = 9
      oApp.lf_applypayments("CC")
    OTHERWISE
      WAIT WINDOW "This feature not available"

    ENDCASE

    **********
    * MAINTAIN
    **********
  CASE ln_mencol = 3
    DO CASE
    ENDCASE

    ***********
    *  REPORTS
    ***********
  CASE ln_mencol = 4
    DO CASE
    CASE ln_menrow = 1
      oApp.lf_printbills("P")
    CASE ln_menrow = 2
      oApp.lf_printbills("V")
    CASE ln_menrow = 3
      oApp.lf_billsummary()
    CASE ln_menrow = 4
      oApp.lf_revenuesummary()
    CASE ln_menrow = 5
      oApp.lf_taxsummary()
    CASE ln_menrow = 6
      oApp.lf_driverslipscommissions()
    CASE ln_menrow = 7
      oApp.lf_driveratmfillcommissions()
    CASE ln_menrow = 8
      oApp.lf_driveratmservicecommissions()
    CASE ln_menrow = 9
      oApp.lf_slsslipcommissions()
    CASE ln_menrow = 10
      oApp.lf_slsatmfillcommissions()
    CASE ln_menrow = 11
      oApp.lf_slsatmservicecommissions()
    CASE ln_menrow = 12
      oApp.lf_payenvelopes()
    CASE ln_menrow = 13
      oApp.lf_dropschedulereport()
    CASE ln_menrow = 14
      *-- ATM Spreadsheet- Invoice to Customer for Rapid
      = oApp.lf_atmbillingspreadsheetdriver("R")
    CASE ln_menrow = 15
      *-- ATM Spreadsheet - Invoice to Rapid
      = oApp.lf_atmbillingspreadsheetdriver("C")
    CASE ln_menrow = 16
      *-- ATM Spreadsheet - Service charges are billed directly to the owner of the ATM - Rapid does see it.
      = oApp.lf_atmbillingspreadsheetdriver("S")
    CASE ln_menrow = 17
      *-- Email Invoices
      = oApp.lf_emailoverdueinvoice()
    CASE ln_menrow = 18
      *-- Cash Receipts Register
      = oApp.lf_cashreceiptsregister()
    CASE ln_menrow = 19
      *-- Invoice Register
      = oApp.lf_invoiceregister()
    CASE ln_menrow = 20
      *-- Aging
      = oApp.lf_araging()
    CASE ln_menrow = 21
      *-- Statements
      = oApp.lf_arstatements()
    CASE ln_menrow = 22
      *-- Manual Invoices
      = oApp.lf_printmanualinvoice()
    CASE ln_menrow = 23
      *-- Applications
      = oApp.lf_applicationregister()
    CASE ln_menrow = 24
      *-- Applications
      = oApp.lf_SBTCashreceiptsegister()
    CASE ln_menrow = 25
      = oApp.lf_emailpdfinvoices()
    CASE ln_menrow = 26
      = oApp.lf_customerhistory()
    CASE ln_menrow = 27
      = oApp.lf_viewcommissionspdf("D")
   CASE ln_menrow = 28
      = oApp.lf_viewcommissionspdf("S")
    ENDCASE
  ENDCASE
ENDDO


IF ll_srs
  RETURN
ELSE
  QUIT
ENDIF

RETURN



*******************
FUNCTION lf_allopen
*******************
LOCAL ll_ok
ll_ok = .T.

*-- Rules
*-- Rules
oApp.openfile(gf_sycrlstf, "a_sycrlst")
SET FILTER TO NOT DELETED()

oApp.openfile(gf_armastf, "a_armast")
oApp.openfile(gf_arcashf, "a_arcash")
oApp.openfile(gf_arcustf, "a_arcust")
oApp.openfile(gf_acrecgf, "a_acrecg")



*-- Sysdata
oApp.openfile(gf_sysdataf, "a_sysdat")
SET FILTER TO NOT DELETED()

= SQLPREPARE(gnConnHandle, "SELECT * FROM acstdt01", ;
  "a_acstdt")
= SQLEXEC(gnConnHandle)
*-- Commit the transactions
ln_commit = SQLCOMMIT(gnConnHandle)
SELECT a_acstdt
INDEX ON storecode TAG store

RETURN ll_ok

*******************
PROCEDURE l_setcomp
*******************
PRIVATE    lc_datapath, lc_baysyspath, lc_baydatapath,ll_cont

ll_cont = .T.
*-- Open compdata
mfile = "compdata"
= oApp.openfile(mfile,"compdata")
SELECT compdata
lc_datapath =  ALLTRIM(compdata.dbfpath)
lc_baysyspath = ALLTRIM(compdata.baypath)

*-- Establish test close variable
oApp.lc_testclose = compdata->testclose
IF  oApp.openfile(lc_baysyspath + "sycdata", "a_sycdata")
  SELECT a_sycdata
  SET ORDER TO compid1
  SEEK gc_compid + "AC"
  IF FOUND()
    lc_baydatapath = ALLTRIM(adosdbf)
  ELSE
    ll_cont = .F.
    DO notice WITH "Unable to locate Pro Series System tables"
  ENDIF
  USE IN a_sycdata
ELSE
  DO notice WITH "Unable to locate Pro Series System tables"
  ll_cont = .F.
ENDIF
IF ll_cont
  *-- SQL Connection name
  gc_sqlserver = UPPER(ALLTRIM(compdata->sqlserver))
  *-- SQL user
  gc_sqluser = ALLTRIM(compdata->sqluser)
  *-- SQL DB
  gc_sqldb = ALLTRIM(compdata->sqldbname)
  *-- SQL Passwd
  gc_sqlpasswd = ALLTRIM(compdata->sqlpasswd)
  gf_sysdataf = lc_baysyspath + "sysdata"
  gf_sycrlstf = lc_baysyspath + "sycrlst"
  gf_armastf  = lc_baysyspath + "\ardata\armast01"
  gf_arcustf  = lc_baysyspath + "\ardata\arcust01"
  gf_arcashf  = lc_baysyspath + "\ardata\arcash01"
  gf_acrecgf  = FULLPATH(CURDIR()) + "\acrecg01"
  oApp.gc_smtpfrom = ALLTRIM(compdata->smtpfrom)
  oApp.gc_smtpserver = ALLTRIM(compdata->mailserver)
  oApp.gc_smtpuser = ALLTRIM(compdata->smtpuser)
  oApp.gc_smtppass = ALLTRIM(compdata->smtppass)
  oApp.gl_mailtest = compdata->mailtest
  oApp.gc_testaddr = ALLTRIM(compdata->testaddr)
  oApp.gc_ftpaddress = ALLTRIM(compdata->ftpaddress)
  oApp.gc_username = ALLTRIM(compdata->username)
  oApp.gc_ftppass = ALLTRIM(compdata->ftppass)


  *-- Establish CDO variables
  oApp.ln_cdoserport = compdata->cdoserport
  oApp.ln_cdoauthen = compdata->cdoauthen
  oApp.ln_cdotimeout = compdata->cdotimeout
  oApp.ll_cdousessl = compdata->cdousessl
  oApp.ll_cdogetver = compdata->cdogetver
  oApp.ln_cdouseport = compdata->cdouseport




ENDIF ll_cont
RETURN ll_cont

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
= SQLPREPARE(gnConnHandle, "SELECT * FROM store WHERE LEFT(storecode,11) = ?lc_storecode ", "curstoreseek")
ln_exec = SQLEXEC(gnConnHandle)
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
**********************
FUNCTION lf_billname
********************
*-- Get store/company name
PARAMETER pc_billstore
pc_billstore = LEFT(pc_billstore,11)
PRIVATE lc_billname, lc_exact
lc_exact = SET("EXACT")
lc_alias = ALIAS()
SET EXACT OFF
lc_billname = "Unident " + ALLTRIM(pc_billstore)
*-- If rightmost six character are blank, use company table
IF EMPTY(RIGHT(pc_billstore,6))
  lc_compcode =  LEFT(pc_billstore,4)
  lc_sqlstring = "SELECT * FROM company WHERE comp_code = ?lc_compcode"
  = SQLPREPARE(gnConnHandle, lc_sqlstring , "curbillcompany")
  ln_exec = SQLEXEC(gnConnHandle)
  IF ln_exec < 1
    WAIT WINDOW "SQL ERROR Has Occurred "
    = AERROR(la_error)
    WAIT WINDOW la_error(1,2)
    ll_cont = .F.
  ENDIF
  IF NOT EOF("curbillcompany")
    lc_billname = IIF(EMPTY(curbillcompany->billname),curbillcompany->name,curbillcompany->billname)
  ELSE
    lc_billname = "Unidentified Customer"
  ENDIF
ELSE
  lc_sqlstring = "SELECT * FROM store WHERE LEFT(storecode,11) = ?pc_billstore"
  = SQLPREPARE(gnConnHandle, lc_sqlstring , "curbillstore")
  ln_exec = SQLEXEC(gnConnHandle)
  IF ln_exec < 1
    WAIT WINDOW "SQL ERROR Has Occurred "
    = AERROR(la_error)
    WAIT WINDOW la_error(1,2)
    ll_cont = .F.
  ENDIF

  IF NOT EOF("curbillstore")
    lc_billname = curbillstore->store_name
  ENDIF
ENDIF
SET EXACT &lc_exact
SELECT &lc_alias
RETURN lc_billname

RETURN lc_return
*****************
FUNCTION l_driver
*****************
*-- Get driver name
PARAMETERS pc_driver
PRIVATE lc_name, ;
  lc_alias, ;
  lc_order, ;
  ln_recno

lc_alias = ALIAS()
lc_order = ORDER()
ln_recno = IIF(EOF(), 0, RECNO())
lc_name = ""

IF USED("curdriver")
  USE IN curdriver
ENDIF
= SQLPREPARE(gnConnHandle, "SELECT * FROM driver WHERE number =?pc_driver", "curdriver")
ll_cont = oApp.lf_execsql(gnConnHandle)
IF NOT EOF("curdriver")
  lc_name = ALLTRIM(curdriver->firstname) + " " + ALLTRIM(curdriver->lastname)
ELSE
  lc_name = "Unidentified Driver"
ENDIF

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

******************
FUNCTION l_billadd
******************
*-- Get store address
PARAMETER pc_billstore
pc_billstore = LEFT(pc_billstore,11)
PRIVATE lc_billadd
lc_alias = ALIAS()

lc_billadd = "Unidentifiable"
lc_sqlstring = "SELECT * FROM store WHERE LEFT(storecode,11) = ?pc_billstore"
= SQLPREPARE(gnConnHandle, lc_sqlstring , "curbillstore")
ln_exec = SQLEXEC(gnConnHandle)
IF ln_exec < 1
  WAIT WINDOW "SQL ERROR Has Occurred "
  = AERROR(la_error)
  WAIT WINDOW la_error(1,2)
  ll_cont = .F.
ENDIF

IF NOT EOF("curbillstore")
  lc_billadd =  ALLTRIM(curbillstore->f_address)
ENDIF
SELECT &lc_alias


RETURN lc_billadd

*******************
FUNCTION l_billcity
*******************
*-- Get store city
PARAMETER pc_billstore
pc_billstore = LEFT(pc_billstore,11)
PRIVATE lc_billcity
lc_alias = ALIAS()

lc_billcity = "Unidentifiable"
lc_sqlstring = "SELECT * FROM store WHERE LEFT(storecode,11) = ?pc_billstore"
= SQLPREPARE(gnConnHandle, lc_sqlstring , "curbillstore")
ln_exec = SQLEXEC(gnConnHandle)
IF ln_exec < 1
  WAIT WINDOW "SQL ERROR Has Occurred "
  = AERROR(la_error)
  WAIT WINDOW la_error(1,2)
  ll_cont = .F.
ENDIF

IF NOT EOF("curbillstore")
  lc_billcity =   ALLTRIM(curbillstore->f_city) + ", " + ALLTRIM(curbillstore->f_state) + " " + ;
    ALLTRIM(curbillstore->f_zip)

ENDIF
SELECT &lc_alias

RETURN lc_billcity

FUNCTION l_salesmn
******************
PARAMETERS pc_salesmn
PRIVATE lc_name
lc_alias = ALIAS()
lc_name = "Unknown"
lc_sqlstring = "select DISTINCT RTRIM(slsname) AS slsname from salesperson where slscode = ?pc_salesmn"
= SQLPREPARE(gnConnHandle, lc_sqlstring , "cursales")
ln_exec = SQLEXEC(gnConnHandle)
IF ln_exec < 1
  WAIT WINDOW "SQL ERROR Has Occurred "
  = AERROR(la_error)
  WAIT WINDOW la_error(1,2)
  ll_cont = .F.
ENDIF
IF NOT EOF("cursales")
  lc_name = cursales->slsname
ENDIF
SELECT &lc_alias
RETURN lc_name
******************
FUNCTION l_nameadd
******************
*-- Get store name and address
PARAMETER pc_billstore
pc_billstore = LEFT(pc_billstore,11)
PRIVATE lc_billadd
lc_alias = ALIAS()

lc_billadd = "Unidentifiable"
lc_sqlstring = "SELECT * FROM store WHERE LEFT(storecode,11) = ?pc_billstore"
= SQLPREPARE(gnConnHandle, lc_sqlstring , "curbillstore")
ln_exec = SQLEXEC(gnConnHandle)
IF ln_exec < 1
  WAIT WINDOW "SQL ERROR Has Occurred "
  = AERROR(la_error)
  WAIT WINDOW la_error(1,2)
  ll_cont = .F.
ENDIF

IF NOT EOF("curbillstore")
  lc_billadd = ALLTRIM(curbillstore->store_name) + ", " + ALLTRIM(curbillstore->f_address)
ENDIF

SELECT &lc_alias
RETURN lc_billadd

*******************
FUNCTION lc_compname
*******************
*-- Get store/company name
PARAMETER pc_compcode
PRIVATE lc_compname, lc_exact
lc_alias = ALIAS()
lc_sqlstring = "SELECT * FROM company WHERE comp_code = ?pc_compcode"
= SQLPREPARE(gnConnHandle, lc_sqlstring , "curcompany")
ln_exec = SQLEXEC(gnConnHandle)
IF ln_exec < 1
  WAIT WINDOW "SQL ERROR Has Occurred "
  = AERROR(la_error)
  WAIT WINDOW la_error(1,2)
  ll_cont = .F.
ENDIF
IF NOT EOF("curcompany")
  lc_compname = curcompany->name
ELSE
  lc_compname = "Unidentified Customer"
ENDIF

SELECT &lc_alias
RETURN lc_compname

*-- This procedure avoids a false error in the project builder. It is never called.
PROCEDURE dummy
PUBLIC ARRAY la_dateranges[1]

