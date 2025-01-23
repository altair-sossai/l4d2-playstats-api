namespace L4D2PlayStats.Core.Modules.Campaigns.Repositories;

public class CampaignRepository : ICampaignRepository
{
    private static readonly List<Campaign> Campaigns =
    [
        new()
        {
            Name = "Dead Center",
            Maps =
            [
                "c1m1_hotel",
                "c1m2_streets",
                "c1m3_mall",
                "c1m4_atrium"
            ]
        },
        new()
        {
            Name = "Dark Carnival",
            Maps =
            [
                "c2m1_highway",
                "c2m2_fairgrounds",
                "c2m3_coaster",
                "c2m4_barns",
                "c2m5_concert"
            ]
        },
        new()
        {
            Name = "Swamp Fever",
            Maps =
            [
                "c3m1_plankcountry",
                "c3m2_swamp",
                "c3m3_shantytown",
                "c3m4_plantation"
            ]
        },
        new()
        {
            Name = "Hard Rain",
            Maps =
            [
                "c4m1_milltown_a",
                "c4m2_sugarmill_a",
                "c4m3_sugarmill_b",
                "c4m4_milltown_b",
                "c4m5_milltown_escape"
            ]
        },
        new()
        {
            Name = "The Parish",
            Maps =
            [
                "c5m1_waterfront",
                "c5m2_park",
                "c5m3_cemetery",
                "c5m4_quarter",
                "c5m5_bridge"
            ]
        },
        new()
        {
            Name = "The Passing",
            Maps =
            [
                "c6m1_riverbank",
                "c6m2_bedlam",
                "c7m1_docks",
                "c7m2_barge"
            ]
        },
        new()
        {
            Name = "The Sacrifice",
            Maps =
            [
                "c7m1_docks",
                "c7m2_barge",
                "c7m3_port"
            ]
        },
        new()
        {
            Name = "No Mercy",
            Maps =
            [
                "c8m1_apartment",
                "c8m2_subway",
                "c8m3_sewers",
                "c8m4_interior",
                "c8m5_rooftop"
            ]
        },
        new()
        {
            Name = "Crash Course",
            Maps =
            [
                "c9m1_alleys",
                "c9m2_lots",
                "c14m1_junkyard",
                "c14m2_lighthouse"
            ]
        },
        new()
        {
            Name = "Death Toll",
            Maps =
            [
                "c10m1_caves",
                "c10m2_drainage",
                "c10m3_ranchhouse",
                "c10m4_mainstreet",
                "c10m5_houseboat"
            ]
        },
        new()
        {
            Name = "Dead Air",
            Maps =
            [
                "c11m1_greenhouse",
                "c11m2_offices",
                "c11m3_garage",
                "c11m4_terminal",
                "c11m5_runway"
            ]
        },
        new()
        {
            Name = "Blood Harvest",
            Maps =
            [
                "c12m1_hilltop",
                "c12m2_traintunnel",
                "c12m3_bridge",
                "c12m4_barn",
                "c12m5_cornfield"
            ]
        },
        new()
        {
            Name = "Cold Stream",
            Maps =
            [
                "c13m1_alpinecreek",
                "c13m2_southpinestream",
                "c13m3_memorialbridge",
                "c13m4_cutthroatcreek"
            ]
        },
        new()
        {
            Name = "The Last Stand",
            Maps =
            [
                "c14m1_junkyard",
                "c14m2_lighthouse"
            ]
        },
        new()
        {
            Name = "Dark Carnival Remix",
            Maps =
            [
                "dkr_m1_motel",
                "dkr_m2_carnival",
                "dkr_m3_tunneloflove",
                "dkr_m4_ferris",
                "dkr_m5_stadium"
            ]
        },
        new()
        {
            Name = "Open Road",
            Maps =
            [
                "x1m1_cliffs",
                "x1m2_path",
                "x1m3_city",
                "x1m4_forest",
                "x1m5_salvation"
            ]
        },
        new()
        {
            Name = "Detour Ahead",
            Maps =
            [
                "cdta_01detour",
                "cdta_02road",
                "cdta_03warehouse",
                "cdta_04onarail",
                "cdta_05finalroad"
            ]
        },
        new()
        {
            Name = "Heaven Can Wait",
            Maps =
            [
                "AirCrash",
                "RiverMotel",
                "OutSkirts",
                "CityHall",
                "BombShelter"
            ]
        },
        new()
        {
            Name = "Carried Off",
            Maps =
            [
                "cwm1_intro",
                "cwm2_warehouse",
                "cwm3_drain",
                "cwm4_building"
            ]
        },
        new()
        {
            Name = "Dam It! The Director's Cut",
            Maps =
            [
                "damitdc1",
                "damitdc2",
                "damitdc3",
                "damitdc4"
            ]
        },
        new()
        {
            Name = "Hard Rain: Downpour",
            Maps =
            [
                "dprm1_milltown_a",
                "dprm2_sugarmill_a",
                "dprm3_sugarmill_b",
                "dprm4_milltown_b",
                "dprm5_milltown_escape"
            ]
        },
        new()
        {
            Name = "Haunted Forest",
            Maps =
            [
                "hf01_theforest",
                "hf02_thesteeple",
                "hf03_themansion",
                "hf04_escape"
            ]
        },
        new()
        {
            Name = "Arena of the Dead 2",
            Maps =
            [
                "jsarena201_town",
                "jsarena202_alley",
                "jsarena203_roof",
                "jsarena204_arena"
            ]
        },
        new()
        {
            Name = "Dead Before Dawn DC",
            Maps =
            [
                "l4d_dbd2dc_anna_is_gone",
                "l4d_dbd2dc_the_mall",
                "l4d_dbd2dc_clean_up",
                "l4d_dbd2dc_undead_center",
                "l4d_dbd2dc_new_dawn"
            ]
        },
        new()
        {
            Name = "I Hate Mountains 2",
            Maps =
            [
                "l4d_ihm01_forest",
                "l4d_ihm02_manor",
                "l4d_ihm03_underground",
                "l4d_ihm04_lumberyard",
                "l4d_ihm05_lakeside"
            ]
        },
        new()
        {
            Name = "The Bloody Moors",
            Maps =
            [
                "l4d_tbm_1",
                "l4d_tbm_2",
                "l4d_tbm_3",
                "l4d_tbm_4",
                "l4d_tbm_5"
            ]
        },
        new()
        {
            Name = "Yama",
            Maps =
            [
                "l4d_yama_1",
                "l4d_yama_2",
                "l4d_yama_3",
                "l4d_yama_4",
                "l4d_yama_5"
            ]
        }
    ];

    public List<Campaign> GetCampaigns()
    {
        return Campaigns;
    }
}