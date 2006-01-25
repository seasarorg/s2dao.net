SET SQL DIALECT 3;

CREATE DATABASE 'C:\S2DEMO.FDB'
PAGE_SIZE 4096
user 'SYSDBA' password 'masterkey'
DEFAULT CHARACTER SET UNICODE_FSS;

/* Table: EMP2, Owner: SYSDBA */

CREATE TABLE "EMP2" 
(
  "EMPNO"	 INTEGER NOT NULL,
  "ENAME"	 VARCHAR(10) CHARACTER SET UNICODE_FSS,
  "DEPTNUM"	 SMALLINT,
 PRIMARY KEY ("EMPNO")
);


/* Use GSEC (Create User)======================================== */
/* \FireBird_1_5\bin\gsec.exe -user sysdba -password masterkey    */
/* GSEC> add S2DEMO -pw s2demo                                    */
/* GSEC> quit                                                     */
/* End GSEC ===================================================== */


/* Grant permissions for this database */
GRANT DELETE, INSERT, SELECT, UPDATE, REFERENCES ON "EMP2" TO S2DEMO;


INSERT INTO EMP2 VALUES(7369,'SMITH',20);
INSERT INTO EMP2 VALUES(7499,'ALLEN',30);