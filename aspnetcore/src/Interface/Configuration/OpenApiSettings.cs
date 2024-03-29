﻿namespace CSC.PublicApi.Interface.Configuration;

public class OpenApiSettings
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Uri? TermsOfService { get; set; }
    public string? ContactName { get; set; }
    public string? ContactEmail { get; set; }
    public Uri? ContactUrl { get; set; }
    
    public string? HttpScheme { get; set; }
    
    public string? BasePath { get; set; }
}