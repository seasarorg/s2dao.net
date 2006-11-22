CREATE DATABASE s2demo
  WITH OWNER = s2demouser
       ENCODING = 'UTF8'
       TABLESPACE = pg_default;

\connect s2demo

CREATE TABLE emp2 (
    empno integer NOT NULL,
    ename character varying(40),
    deptnum smallint
);

ALTER TABLE ONLY emp2
    ADD CONSTRAINT emp2_pkey PRIMARY KEY (empno);

CREATE SEQUENCE "SEQ_SEQTABLE"
  INCREMENT 1
  MINVALUE 1
  MAXVALUE 9223372036854775807
  START 1
  CACHE 1;
ALTER TABLE "SEQ_SEQTABLE" OWNER TO s2demouser;

CREATE TABLE "SequenceTable"
(
  "SeqCol" int4 NOT NULL DEFAULT nextval('SEQ_SEQTABLE'::regclass)
) 
WITHOUT OIDS;
ALTER TABLE "SequenceTable" OWNER TO s2demouser;

INSERT INTO emp2 (empno, ename, deptnum) VALUES (7369, 'SMITH', 20);
INSERT INTO emp2 (empno, ename, deptnum) VALUES (7499, 'ALLEN', 30);

