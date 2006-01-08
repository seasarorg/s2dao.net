SET SQL DIALECT 3;

CREATE DATABASE 'C:\S2DEMO.FDB'
PAGE_SIZE 4096
DEFAULT CHARACTER SET UNICODE_FSS;

/* Table: EMP2, Owner: SYSDBA */

CREATE TABLE "EMP2" 
(
  "EMPNO"	 INTEGER NOT NULL,
  "ENAME"	 VARCHAR(10) CHARACTER SET UNICODE_FSS,
  "DEPTNUM"	 SMALLINT,
 PRIMARY KEY ("EMPNO")
);


/* Grant Roles for this database */


/* Grant permissions for this database */

GRANT DELETE, INSERT, SELECT, UPDATE, REFERENCES ON "EMP2" TO S2DEMO;