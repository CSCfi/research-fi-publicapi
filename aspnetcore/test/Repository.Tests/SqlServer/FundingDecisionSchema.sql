-- Query surface from DatabaseContext/ApiDbContext.cs, not a warehouse migration.
-- Applied after FundingCallSchema.sql; only funding-decision projection/filter relationships are added.
CREATE TABLE br_call_programme_dim_call_programme (
    dim_call_programme_id int NOT NULL REFERENCES dim_call_programme(id),
    dim_call_programme_id2 int NOT NULL REFERENCES dim_call_programme(id),
    PRIMARY KEY (dim_call_programme_id, dim_call_programme_id2)
);

CREATE TABLE dim_known_person (
    id int NOT NULL PRIMARY KEY
);

CREATE TABLE dim_name (
    id bigint NOT NULL PRIMARY KEY,
    first_names nvarchar(255) NULL,
    last_name nvarchar(255) NULL,
    dim_known_person_id_confirmed_identity int NOT NULL REFERENCES dim_known_person(id)
);

CREATE TABLE dim_pid (
    id int NOT NULL PRIMARY KEY,
    pid_content nvarchar(255) NOT NULL,
    pid_type nvarchar(255) NOT NULL,
    dim_organization_id int NOT NULL REFERENCES dim_organization(id),
    dim_known_person_id int NOT NULL REFERENCES dim_known_person(id)
);

CREATE TABLE dim_funding_decision (
    id int NOT NULL PRIMARY KEY,
    dim_date_id_start int NOT NULL REFERENCES dim_date(id),
    dim_date_id_end int NOT NULL REFERENCES dim_date(id),
    dim_call_programme_id int NOT NULL REFERENCES dim_call_programme(id),
    dim_type_of_funding_id int NOT NULL REFERENCES dim_referencedata(id),
    dim_organization_id_funder int NULL REFERENCES dim_organization(id),
    dim_funding_decision_id_parent_decision int NOT NULL REFERENCES dim_funding_decision(id),
    name_fi nvarchar(max) NULL,
    name_sv nvarchar(max) NULL,
    name_en nvarchar(max) NULL,
    acronym varchar(255) NULL,
    description_fi nvarchar(max) NULL,
    description_sv nvarchar(max) NULL,
    description_en nvarchar(max) NULL,
    amount_in_EUR decimal(18, 2) NOT NULL,
    funder_project_number nvarchar(255) NULL,
    source_id nvarchar(255) NOT NULL,
    source_description nvarchar(255) NULL,
    created datetime NULL,
    modified datetime NULL
);

CREATE TABLE br_funding_consortium_participation (
    dim_funding_decision_id int NOT NULL REFERENCES dim_funding_decision(id),
    dim_organizationid int NOT NULL REFERENCES dim_organization(id),
    role_in_consortium nvarchar(255) NULL,
    share_of_funding_in_EUR decimal(18, 2) NULL,
    PRIMARY KEY (dim_funding_decision_id, dim_organizationid)
);

CREATE TABLE br_participates_in_funding_group (
    dim_funding_decisionid int NOT NULL REFERENCES dim_funding_decision(id),
    dim_name_id bigint NOT NULL REFERENCES dim_name(id),
    dim_organization_id int NOT NULL REFERENCES dim_organization(id),
    source_id nvarchar(255) NOT NULL,
    role_in_funding_group nvarchar(255) NULL,
    share_of_funding_in_EUR decimal(18, 2) NULL,
    PRIMARY KEY (dim_funding_decisionid, dim_name_id)
);

CREATE TABLE fact_dim_referencedata_field_of_science (
    dim_referencedata_id int NOT NULL REFERENCES dim_referencedata(id),
    dim_research_dataset_id int NOT NULL DEFAULT -1,
    dim_known_person_id int NOT NULL DEFAULT -1 REFERENCES dim_known_person(id),
    dim_publication_id int NOT NULL DEFAULT -1,
    dim_research_activity_id int NOT NULL DEFAULT -1,
    dim_funding_decision_id int NOT NULL REFERENCES dim_funding_decision(id),
    dim_infrastructure_id int NOT NULL DEFAULT -1,
    PRIMARY KEY (dim_referencedata_id, dim_research_dataset_id, dim_known_person_id,
        dim_publication_id, dim_research_activity_id, dim_funding_decision_id, dim_infrastructure_id)
);

CREATE TABLE dim_keyword (
    id int NOT NULL PRIMARY KEY,
    keyword nvarchar(255) NULL,
    language nvarchar(255) NULL,
    scheme nvarchar(255) NULL
);

CREATE TABLE br_keyword_dim_funding_decision (
    dim_keyword_id int NOT NULL REFERENCES dim_keyword(id),
    dim_funding_decision_id int NOT NULL REFERENCES dim_funding_decision(id),
    PRIMARY KEY (dim_keyword_id, dim_funding_decision_id)
);

CREATE TABLE dim_word_cluster (
    id int NOT NULL PRIMARY KEY
);

CREATE TABLE dim_mined_words (
    id int NOT NULL PRIMARY KEY,
    word nvarchar(255) NULL
);

CREATE TABLE br_word_cluster_dim_funding_decision (
    dim_word_cluster_id int NOT NULL REFERENCES dim_word_cluster(id),
    dim_funding_decision_id int NOT NULL REFERENCES dim_funding_decision(id),
    PRIMARY KEY (dim_word_cluster_id, dim_funding_decision_id)
);

CREATE TABLE br_words_define_a_cluster (
    dim_mined_words_id int NOT NULL REFERENCES dim_mined_words(id),
    dim_word_cluster_id int NOT NULL REFERENCES dim_word_cluster(id),
    PRIMARY KEY (dim_mined_words_id, dim_word_cluster_id)
);

INSERT INTO br_call_programme_dim_call_programme VALUES (10, -1), (10, 20), (20, 30), (30, 40);
INSERT INTO dim_organization (id, organization_id, name_en) VALUES (-1, N'sentinel', NULL);
INSERT INTO dim_known_person VALUES (-1), (601), (602);
INSERT INTO dim_name VALUES
    (701, N'Synthetic parent', N'Contributor', 601),
    (702, N'Synthetic child', N'Contributor', 602);
INSERT INTO dim_pid VALUES
    (801, N'999999901', N'PIC', 101, -1),
    (802, N'999999902', N'PIC', 101, -1),
    (803, N'PIC', N'businessid', 102, -1),
    (804, N'synthetic-parent-orcid', N'Orcid', -1, 601),
    (805, N'synthetic-child-orcid', N'Orcid', -1, 602);

INSERT INTO dim_funding_decision
    (id, dim_date_id_start, dim_date_id_end, dim_call_programme_id, dim_type_of_funding_id,
     dim_organization_id_funder, dim_funding_decision_id_parent_decision,
     name_en, amount_in_EUR, source_id, source_description, created, modified) VALUES
    (-1, -1, -1, -1, 201, NULL, -1, N'Sentinel', 0, N'sentinel', N'domestic', NULL, NULL),
    (100, 1, 2, 10, 201, 102, -1, N'Domestic decision without PIC', 100.25, N'decision-100', N'domestic', '20260101', '20260102'),
    (110, 1, 2, 10, 201, 101, 100, N'EU decision with consortium PIC', 12345.67, N'decision-110', N'eu_funding', '20260101', '20260102'),
    (120, 1, 2, 10, 201, 101, -1, N'EU decision without consortium', 0, N'decision-120', N'eu_funding', NULL, NULL),
    (130, 1, 2, 10, 201, 102, -1, N'EU consortium without PIC type', 0, N'decision-130', N'eu_funding', NULL, NULL),
    (140, 1, 2, 10, 201, 101, -1, N'EU funder PIC is insufficient', 0, N'decision-140', N'eu_funding', NULL, NULL),
    (150, 1, 2, 10, 201, 101, -1, N'EU mixed consortium', 200.50, N'decision-150', N'eu_funding', NULL, NULL),
    (160, -1, -1, -1, 201, NULL, -1, N'Unknown source', 0, N'decision-160', NULL, NULL, NULL),
    (170, -1, -1, -1, 201, NULL, -1, N'Domestic sentinel dates', 0, N'decision-170', N'domestic', NULL, NULL);

UPDATE dim_funding_decision SET
    name_fi = N'Synteettinen paatos', name_sv = N'Syntetiskt beslut', acronym = 'SYN',
    description_fi = N'Synteettinen kuvaus', description_sv = N'Syntetisk beskrivning',
    description_en = N'Synthetic description', funder_project_number = N'SYN-110'
WHERE id = 110;

UPDATE dim_funding_decision SET created = '20260101', modified = '20260102'
WHERE created IS NULL;

INSERT INTO br_funding_consortium_participation VALUES
    (100, 102, N'domestic member', 100.25),
    (110, 101, N'coordinator', 12345.67),
    (130, 102, N'member', 0),
    (140, 102, N'member', 0),
    (150, 101, N'coordinator', 100.25),
    (150, 102, N'member', 100.25);
INSERT INTO br_participates_in_funding_group VALUES
    (100, 701, 102, N'parent-contribution', N'parent role', 100.25),
    (110, 702, 101, N'decision-110', N'child role', 234.56);
INSERT INTO fact_dim_referencedata_field_of_science
    (dim_referencedata_id, dim_funding_decision_id) VALUES (201, 110), (202, 110);
INSERT INTO dim_keyword VALUES
    (901, N'Synthetic research field', N'en', N'Tutkimusala'),
    (902, N'Excluded keyword', N'en', N'Other');
INSERT INTO br_keyword_dim_funding_decision VALUES (901, 110), (902, 110);
INSERT INTO dim_word_cluster VALUES (1001);
INSERT INTO dim_mined_words VALUES (1101, N'synthetic topic one'), (1102, N'synthetic topic two');
INSERT INTO br_word_cluster_dim_funding_decision VALUES (1001, 110);
INSERT INTO br_words_define_a_cluster VALUES (1101, 1001), (1102, 1001);
