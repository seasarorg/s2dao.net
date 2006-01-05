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

INSERT INTO emp2 (empno, ename, deptnum) VALUES (7369, 'SMITH', 20);
INSERT INTO emp2 (empno, ename, deptnum) VALUES (7499, 'ALLEN', 30);

