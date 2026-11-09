-- Query surface from DatabaseContext/ApiDbContext.cs, not a warehouse migration.
-- Only columns and constraints used by FundingCallProfile/GetAll/GetChunk are provisioned.
CREATE TABLE dim_date (
    id int NOT NULL PRIMARY KEY,
    [year] int NOT NULL,
    [month] int NOT NULL,
    [day] int NOT NULL
);

CREATE TABLE dim_organization (
    id int NOT NULL PRIMARY KEY,
    organization_id nvarchar(255) NULL,
    name_fi nvarchar(255) NULL,
    name_sv nvarchar(255) NULL,
    name_en nvarchar(255) NULL
);

CREATE TABLE dim_referencedata (
    id int NOT NULL PRIMARY KEY,
    code_value nvarchar(511) NULL,
    name_fi nvarchar(255) NULL,
    name_sv nvarchar(255) NULL,
    name_en nvarchar(255) NULL
);

CREATE TABLE dim_call_programme (
    id int NOT NULL PRIMARY KEY,
    dim_date_id_due int NOT NULL REFERENCES dim_date(id),
    dim_date_id_open int NOT NULL REFERENCES dim_date(id),
    is_open_call bit NOT NULL,
    name_fi nvarchar(511) NULL,
    name_sv nvarchar(511) NULL,
    name_en nvarchar(511) NULL,
    description_fi nvarchar(max) NULL,
    description_sv nvarchar(max) NULL,
    description_en nvarchar(max) NULL,
    application_terms_fi nvarchar(max) NULL,
    application_terms_sv nvarchar(max) NULL,
    application_terms_en nvarchar(max) NULL,
    contact_information nvarchar(max) NULL,
    source_id nvarchar(255) NOT NULL,
    source_description nvarchar(255) NULL,
    source_programme_id nvarchar(55) NULL,
    abbreviation nvarchar(511) NULL,
    eu_call_id nvarchar(511) NULL
);

CREATE TABLE br_organizations_fund_call_programmes (
    dim_organizationid int NOT NULL REFERENCES dim_organization(id),
    dim_call_programmeid int NOT NULL REFERENCES dim_call_programme(id),
    PRIMARY KEY (dim_organizationid, dim_call_programmeid)
);

CREATE TABLE br_dim_referencedata_dim_call_programme (
    dim_call_programme_id int NOT NULL REFERENCES dim_call_programme(id),
    dim_referencedata_id int NOT NULL REFERENCES dim_referencedata(id),
    PRIMARY KEY (dim_call_programme_id, dim_referencedata_id)
);

CREATE TABLE dim_web_link (
    id int NOT NULL PRIMARY KEY,
    url nvarchar(511) NULL,
    link_type nvarchar(255) NULL,
    language_variant nvarchar(255) NULL,
    dim_organization_id int NULL REFERENCES dim_organization(id),
    dim_call_programme_id int NULL REFERENCES dim_call_programme(id)
);

INSERT INTO dim_date (id, [year], [month], [day]) VALUES
    (-1, 0, 0, 0),
    (1, 2026, 1, 15),
    (2, 2026, 2, 28);

INSERT INTO dim_organization (id, organization_id, name_fi, name_sv, name_en) VALUES
    (101, N'foundation-one', N'Synteettinen rahasto', N'Syntetisk fond', N'Synthetic foundation'),
    (102, N'foundation-two', NULL, NULL, N'Second synthetic foundation');

INSERT INTO dim_referencedata (id, code_value, name_fi, name_sv, name_en) VALUES
    (201, N'SYNTHETIC-A', N'Ala A', N'Kategori A', N'Field A'),
    (202, N'SYNTHETIC-B', NULL, NULL, N'Field B');

INSERT INTO dim_call_programme
    (id, dim_date_id_due, dim_date_id_open, is_open_call, name_en, source_id,
     description_en, application_terms_en, contact_information, source_description,
     source_programme_id, abbreviation, eu_call_id) VALUES
    (-1, -1, -1, 1, N'Sentinel', N'sentinel', NULL, NULL, NULL, NULL, NULL, NULL, NULL),
    (10, 2, 1, 1, N'Dated synthetic call', N'call-10', N'Synthetic description',
     N'Synthetic terms', N'Synthetic contact', N'synthetic', N'programme-10', N'SYN', N'EU-SYN'),
    (20, 2, 1, 0, N'Not an open call', N'call-20', NULL, NULL, NULL, NULL, NULL, NULL, NULL),
    (30, 2, 1, 1, N'No funding organization', N'call-30', NULL, NULL, NULL, NULL, NULL, NULL, NULL),
    (40, -1, -1, 1, N'Continuous synthetic call', N'call-40', NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO br_organizations_fund_call_programmes (dim_organizationid, dim_call_programmeid) VALUES
    (101, -1), (101, 10), (102, 10), (101, 20), (101, 40);

INSERT INTO br_dim_referencedata_dim_call_programme (dim_call_programme_id, dim_referencedata_id) VALUES
    (10, 201), (10, 202);

INSERT INTO dim_web_link
    (id, url, link_type, language_variant, dim_organization_id, dim_call_programme_id) VALUES
    (1, N'https://example.invalid/apply/fi', N'ApplicationURL', N'fi', NULL, 10),
    (2, N'https://example.invalid/apply/sv', N'ApplicationURL', N'sv', NULL, 10),
    (3, N'https://example.invalid/apply/en', N'ApplicationURL', N'en', NULL, 10),
    (4, N'https://example.invalid/not-an-application', N'Other', N'en', NULL, 10),
    (5, N'https://example.invalid/foundation', N'FoundationURL', NULL, 101, NULL),
    (6, N'https://example.invalid/not-a-foundation', N'Other', NULL, 101, NULL);
