namespace L4D2PlayStats.Core.Modules.Campaigns.Repositories;

public interface ICampaignRepository
{
    List<Campaign> GetCampaigns();
}

public class CampaignRepository : ICampaignRepository
{
    private static readonly List<Campaign> Campaigns = new()
    {
        new Campaign
        {
            Name = "Dead Center",
            Maps = new List<string>
            {
                "c1m1_hotel",
                "c1m2_streets",
                "c1m3_mall",
                "c1m4_atrium"
            }
        },
        new Campaign
        {
            Name = "Dark Carnival",
            Maps = new List<string>
            {
                "c2m1_highway",
                "c2m2_fairgrounds",
                "c2m3_coaster",
                "c2m4_barns",
                "c2m5_concert"
            }
        },
        new Campaign
        {
            Name = "Swamp Fever",
            Maps = new List<string>
            {
                "c3m1_plankcountry",
                "c3m2_swamp",
                "c3m3_shantytown",
                "c3m4_plantation"
            }
        },
        new Campaign
        {
            Name = "Hard Rain",
            Maps = new List<string>
            {
                "c4m1_milltown_a",
                "c4m2_sugarmill_a",
                "c4m3_sugarmill_b",
                "c4m4_milltown_b",
                "c4m5_milltown_escape"
            }
        },
        new Campaign
        {
            Name = "The Parish",
            Maps = new List<string>
            {
                "c5m1_waterfront",
                "c5m2_park",
                "c5m3_cemetery",
                "c5m4_quarter",
                "c5m5_bridge"
            }
        },
        new Campaign
        {
            Name = "The Passing",
            Maps = new List<string>
            {
                "c6m1_riverbank",
                "c6m2_bedlam",
                "c6m3_port"
            }
        },
        new Campaign
        {
            Name = "The Sacrifice",
            Maps = new List<string>
            {
                "c7m1_docks",
                "c7m2_barge",
                "c7m3_port"
            }
        },
        new Campaign
        {
            Name = "No Mercy",
            Maps = new List<string>
            {
                "c8m1_apartment",
                "c8m2_subway",
                "c8m3_sewers",
                "c8m4_interior",
                "c8m5_rooftop"
            }
        },
        new Campaign
        {
            Name = "Crash Course",
            Maps = new List<string>
            {
                "c9m1_alleys",
                "c9m2_lots"
            }
        },
        new Campaign
        {
            Name = "Death Toll",
            Maps = new List<string>
            {
                "c10m1_caves",
                "c10m2_drainage",
                "c10m3_ranchhouse",
                "c10m4_mainstreet",
                "c10m5_houseboat"
            }
        },
        new Campaign
        {
            Name = "Dead Air",
            Maps = new List<string>
            {
                "c11m1_greenhouse",
                "c11m2_offices",
                "c11m3_garage",
                "c11m4_terminal",
                "c11m5_runway"
            }
        },
        new Campaign
        {
            Name = "Blood Harvest",
            Maps = new List<string>
            {
                "c12m1_hilltop",
                "c12m2_traintunnel",
                "c12m3_bridge",
                "c12m4_barn",
                "c12m5_cornfield"
            }
        },
        new Campaign
        {
            Name = "Cold Stream",
            Maps = new List<string>
            {
                "c13m1_alpinecreek",
                "c13m2_southpinestream",
                "c13m3_memorialbridge",
                "c13m4_cutthroatcreek"
            }
        },
        new Campaign
        {
            Name = "The Last Stand",
            Maps = new List<string>
            {
                "c14m1_junkyard",
                "c14m2_lighthouse"
            }
        }
    };

    public List<Campaign> GetCampaigns()
    {
        return Campaigns;
    }
}