﻿using Microsoft.AspNetCore.Mvc;

namespace CSC.PublicApi.Interface.Models;

public class GetInfrastructuresQueryParameters : PaginationQueryParameters
{
    [FromQuery(Name = "name")]
    public string? Name { get; set; }
}