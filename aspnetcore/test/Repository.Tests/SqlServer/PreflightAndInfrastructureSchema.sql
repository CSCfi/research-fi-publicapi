-- Query surface from DatabasePreflightCheck and infrastructure repositories;
-- this is not a warehouse migration.
CREATE TABLE dim_publication (
    id int NOT NULL PRIMARY KEY
);

CREATE TABLE dim_research_dataset (
    id int NOT NULL PRIMARY KEY
);

CREATE TABLE fact_contribution (
    dim_publication_id int NOT NULL
);

INSERT INTO dim_publication (id) VALUES (-1), (1), (2), (3), (4), (5);
INSERT INTO dim_research_dataset (id) VALUES (-1), (1);
INSERT INTO fact_contribution (dim_publication_id) VALUES (-1), (1), (2), (3), (4);

ALTER TABLE dim_pid ADD dim_infrastructure_id int NOT NULL
    CONSTRAINT DF_dim_pid_dim_infrastructure_id_test DEFAULT (-1);
ALTER TABLE dim_referencedata ADD
    code_scheme nvarchar(511) NOT NULL
        CONSTRAINT DF_dim_referencedata_code_scheme_test DEFAULT (N''),
    source_id nvarchar(255) NOT NULL
        CONSTRAINT DF_dim_referencedata_source_id_test DEFAULT (N'test-reference'),
    source_description nvarchar(255) NOT NULL
        CONSTRAINT DF_dim_referencedata_source_description_test DEFAULT (N'test'),
    created datetime NULL,
    modified datetime NULL,
    state nvarchar(255) NULL,
    dim_referencedata_id int NOT NULL CONSTRAINT DF_dim_referencedata_id_test DEFAULT (-1),
    [order] int NULL;
ALTER TABLE dim_organization ADD
    dim_organization_broader int NULL,
    dim_sectorid int NOT NULL CONSTRAINT DF_dim_organization_dim_sectorid_test DEFAULT (-1),
    organization_active bit NULL,
    local_organization_unit_Id nvarchar(255) NULL,
    local_organization_sector nvarchar(255) NULL,
    organization_background nvarchar(max) NULL,
    organization_type nvarchar(255) NULL,
    name_variants nvarchar(max) NULL,
    name_und nvarchar(4000) NULL,
    country_code nvarchar(255) NULL,
    established datetime NULL,
    visiting_address nvarchar(max) NULL,
    postal_address nvarchar(max) NULL,
    staff_count_as_fte int NULL,
    degree_count_bsc int NULL,
    degree_count_msc int NULL,
    degree_count_lic int NULL,
    degree_count_phd int NULL,
    source_id nvarchar(255) NOT NULL
        CONSTRAINT DF_dim_organization_source_id_test DEFAULT (N'test-organization'),
    source_description nvarchar(255) NULL,
    created datetime NULL,
    modified datetime NULL,
    dim_registered_data_source_id int NOT NULL CONSTRAINT DF_dim_organization_data_source_test DEFAULT (-1),
    research_fi_visibility_limited bit NOT NULL
        CONSTRAINT DF_dim_organization_visibility_test DEFAULT (0);
ALTER TABLE fact_contribution ADD
    dim_infrastructure_id int NOT NULL CONSTRAINT DF_fact_contribution_dim_infrastructure_id_test DEFAULT (-1),
    dim_organization_id int NOT NULL CONSTRAINT DF_fact_contribution_dim_organization_id_test DEFAULT (-1),
    contribution_type nvarchar(50) NOT NULL CONSTRAINT DF_fact_contribution_type_test DEFAULT (N'');

CREATE TABLE dim_infrastructure (
    id int NOT NULL PRIMARY KEY,
    local_identifier nvarchar(255) NULL,
    acronym varchar(255) NULL,
    finland_roadmap bit NULL,
    responsible_organization_id int NOT NULL,
    dim_start_date int NOT NULL,
    dim_end_date int NOT NULL,
    dim_registered_data_source_id int NOT NULL,
    source_id nvarchar(255) NOT NULL,
    source_description nvarchar(255) NULL,
    created datetime NULL,
    modified datetime NULL
);

CREATE TABLE dim_descriptive_item (
    id int NOT NULL PRIMARY KEY,
    dim_start_date int NOT NULL,
    dim_end_date int NULL,
    descriptive_item nvarchar(max) NOT NULL,
    descriptive_item_type nvarchar(255) NOT NULL,
    descriptive_item_language varchar(255) NOT NULL,
    source_id nvarchar(255) NOT NULL,
    source_description nvarchar(255) NULL,
    created datetime NULL,
    modified datetime NULL,
    dim_research_project_id int NOT NULL,
    dim_registered_data_source_id int NOT NULL,
    dim_publication_id int NOT NULL,
    dim_research_dataset_id int NOT NULL,
    dim_infrastructure_id int NOT NULL,
    dim_service_id int NOT NULL,
    dim_research_data_catalog_id int NOT NULL
);

CREATE TABLE dim_service (
    id int NOT NULL PRIMARY KEY,
    local_identifier nvarchar(255) NULL,
    source_id nvarchar(255) NOT NULL,
    source_description nvarchar(255) NULL,
    created datetime NULL,
    modified datetime NULL,
    dim_infrastructure_id int NOT NULL,
    start_date int NOT NULL,
    end_date int NOT NULL
);

CREATE TABLE dim_contact_information (
    id int NOT NULL PRIMARY KEY,
    contact_label nvarchar(255) NOT NULL,
    dim_infrastructure_id int NOT NULL,
    dim_service_id int NOT NULL,
    source_id nvarchar(255) NULL,
    source_description nvarchar(255) NULL,
    created datetime NULL,
    modified datetime NULL
);

CREATE TABLE dim_address (
    id int NOT NULL PRIMARY KEY,
    dim_contact_information_id int NOT NULL,
    street nvarchar(255) NULL,
    premise nvarchar(255) NULL,
    post_office_box nvarchar(255) NULL,
    postalCode nvarchar(255) NULL,
    locality_fi nvarchar(255) NULL,
    locality_sv nvarchar(255) NULL,
    locality_en nvarchar(255) NULL,
    country_code int NULL,
    address_type nvarchar(255) NOT NULL,
    source_id nvarchar(255) NULL,
    source_description nvarchar(255) NULL,
    created datetime NULL,
    modified datetime NULL
);

CREATE TABLE dim_email_addrress (
    id int NOT NULL PRIMARY KEY,
    email nvarchar(255) NULL,
    source_id nvarchar(255) NOT NULL,
    source_description nvarchar(255) NULL,
    created datetime NULL,
    modified datetime NULL,
    dim_registered_data_source_id int NOT NULL,
    dim_known_person_id int NOT NULL,
    dim_contact_information_id int NOT NULL
);

CREATE TABLE dim_telephone_number (
    id int NOT NULL PRIMARY KEY,
    telephone_number nvarchar(255) NULL,
    source_id nvarchar(255) NOT NULL,
    source_description nvarchar(255) NULL,
    created datetime NULL,
    modified datetime NULL,
    dim_registered_data_source_id int NOT NULL,
    dim_known_person_id int NOT NULL,
    dim_contact_information_id int NOT NULL
);

ALTER TABLE dim_web_link ADD
    link_label nvarchar(255) NULL,
    source_description nvarchar(255) NULL,
    created datetime NULL,
    modified datetime NULL,
    dim_service_id int NULL,
    dim_infrastructure_id int NULL;

CREATE TABLE dim_international_infra (
    id int NOT NULL PRIMARY KEY,
    name_en nvarchar(255) NOT NULL,
    unlinked_identifier nvarchar(255) NULL,
    weblink nvarchar(511) NULL,
    source_id nvarchar(255) NOT NULL,
    source_description nvarchar(255) NULL,
    created datetime NULL,
    modified datetime NULL
);

CREATE TABLE fact_relation (
    relation_type_code int NOT NULL,
    from_publication_id int NOT NULL,
    from_research_dataset_id int NOT NULL,
    from_international_infra_id int NOT NULL,
    from_infrastructure_id int NOT NULL,
    to_research_dataset_id int NOT NULL,
    to_international_infra_id int NOT NULL,
    to_publication_id int NOT NULL,
    to_infrastructure_id int NOT NULL,
    start_date int NOT NULL,
    end_date int NOT NULL,
    dim_registered_data_source_id int NOT NULL,
    created datetime NULL,
    modified datetime NULL,
    source_description nvarchar(255) NULL,
    source_id nvarchar(255) NULL,
    valid_relation bit NULL
);

CREATE TABLE fact_referencedata (
    dim_referencedata_id int NOT NULL,
    dim_research_dataset_id int NOT NULL DEFAULT -1,
    dim_infrastructure_id int NOT NULL,
    dim_publication_id int NOT NULL DEFAULT -1,
    dim_research_activity_id int NOT NULL DEFAULT -1,
    dim_funding_decision_id int NOT NULL DEFAULT -1,
    dim_call_programme_id int NOT NULL DEFAULT -1,
    dim_service_id int NOT NULL DEFAULT -1
);
