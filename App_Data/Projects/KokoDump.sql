--
-- PostgreSQL database dump
--

-- Dumped from database version 16.2 (Debian 16.2-1.pgdg120+2)
-- Dumped by pg_dump version 16.2 (Debian 16.2-1.pgdg120+2)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: test_table; Type: TABLE; Schema: public; Owner: Koko@example.com
--

CREATE TABLE public.test_table (
    id integer NOT NULL,
    st2 character varying(100)
);


ALTER TABLE public.test_table OWNER TO "Koko@example.com";

--
-- Data for Name: test_table; Type: TABLE DATA; Schema: public; Owner: Koko@example.com
--

COPY public.test_table (id, st2) FROM stdin;
1	тест
2	test
3	ряяя
\.


--
-- Name: test_table test_table_pkey; Type: CONSTRAINT; Schema: public; Owner: Koko@example.com
--

ALTER TABLE ONLY public.test_table
    ADD CONSTRAINT test_table_pkey PRIMARY KEY (id);


--
-- PostgreSQL database dump complete
--

