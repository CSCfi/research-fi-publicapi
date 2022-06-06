﻿namespace Api.Models.ResearchDataset
{
    public class ResearchDataset
    {
        public string? NameFi { get; set; }
        public string? NameSv { get; set; }
        public string? NameEn { get; set; }
        public string? DescriptionFi { get; set; }
        public string? DescriptionSv { get; set; }

        public string? DescriptionEn { get; set; }

        public DateTime? DatasetCreated { get; set; }

        //TODO: tekijät

        public List<FieldOfScience>? FieldOfSciences { get; set; }

        public List<Language>? Languages { get; set; }

        public string? AccessType { get; set; }

        public List<License>? Licences { get; set; }

        public List<Keyword>? Keywords { get; set; }

        // TODO: liittyvät aineistot

        // TODO: muut liittyvät tuotokset

        public Version? VersionSet { get; set; }

        public List<PreferredIdentifier>? PreferredIdentifiers { get; set; }

        // TODO: paikallinen tunniste

        public string? FairDataUrl { get; set; }

        // TODO: muu linkki

        // TODO: tutkimusaineistotiedon lähde

    }


}
