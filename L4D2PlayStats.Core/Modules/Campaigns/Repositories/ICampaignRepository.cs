﻿namespace L4D2PlayStats.Core.Modules.Campaigns.Repositories;

public interface ICampaignRepository
{
    IEnumerable<Campaign> GetCampaigns();
}