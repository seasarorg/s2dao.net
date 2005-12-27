CREATE USER "S2DEMOUSER" PROFILE "DEFAULT" IDENTIFIED BY "s2demouser" DEFAULT TABLESPACE "USERS" TEMPORARY TABLESPACE "TEMP" ACCOUNT UNLOCK;
GRANT "CONNECT" TO "S2DEMOUSER";

GRANT UNLIMITED TABLESPACE TO "S2DEMOUSER";
GRANT CREATE TABLE TO "S2DEMOUSER";

CREATE TABLE "S2DEMOUSER"."EMP2" ( "EMPNO" INTEGER NOT NULL , "ENAME" VARCHAR2(10), "DEPTNUM" INTEGER, PRIMARY KEY ("EMPNO") VALIDATE );

INSERT INTO S2DEMOUSER.EMP2 VALUES(7369,'SMITH',20);
INSERT INTO S2DEMOUSER.EMP2 VALUES(7499,'ALLEN',30);