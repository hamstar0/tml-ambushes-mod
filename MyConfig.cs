using HamstarHelpers.Classes.UI.ModConfig;
using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace Ambushes {
	class MyFloatInputElement : FloatInputElement { }




	public class AmbushesConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;


		////

		[Header( "Debug settings" )]
		public bool DebugModeInfo { get; set; } = false;
		public bool DebugModeMapCheat { get; set; } = false;
		public bool DebugModeInfoBrambles { get; set; } = false;
		public bool DebugModeInfoSpawns { get; set; } = false;


		////

		[Header( "Ambush world settings" )]
		[Range( 0, 1024 )]
		[DefaultValue( 128 )]
		public int TinyWorldInitialAmbushes { get; set; } = 160;    // SmallWorldInitialAmbushes / 2

		[Range( 0, 1024 )]
		[DefaultValue( 192 )]
		public int SmallWorldInitialAmbushes { get; set; } = 256;  // 4200 x 1200 = 5040000

		[Range( 0, 2048 )]
		[DefaultValue( 384 )]
		public int MediumWorldInitialAmbushes { get; set; } = 512; // 6400 x 1800 = 11520000

		[Range( 0, 2024 )]
		[DefaultValue( 768 )]
		public int LargeWorldInitialAmbushes { get; set; } = 1024;  // 8400 x 2400 = 20160000

		[Range( 0, 4048 )]
		[DefaultValue( 1280 )]
		public int HugeWorldInitialAmbushes { get; set; } = 2048;

		[Range( 0, 10000 )]
		[DefaultValue( 80 )]
		public int AmbushPlayerNearbyNeededTileRadius { get; set; } = 80;


		/*[Range( 1, 10000 )]
		[DefaultValue( 10 )]
		public int AmbushInitialGenerationSlowness { get; set; } = 10;*/

		[Range( 1, 60 * 60 * 60 * 24 )]
		[DefaultValue( 60 * 60 * 10 )]
		[Label( "Duration (in ticks) until a used ambush re-randomizes" )]
		public int AmbushRegenTickRate { get; set; } = 60 * 60 * 10;    // 10 minutes

		[Range( 1, 50000 )]
		[DefaultValue( 64 )]
		public int MinimumAmbushTileSeparation { get; set; } = 64;

		////

		[Header( "Ambush settings" )]
		[Range( 1, 1000 )]
		[DefaultValue( 8 )]
		[ReloadRequired]
		public int AmbushTriggerRadiusTiles { get; set; } = 8;

		[Range( 1, 1000 )]
		[DefaultValue( 40 )]
		[ReloadRequired]
		public int AmbushEntrapmentRadius { get; set; } = 40;


		[Range( 1, 60*60*60*24 )]
		[DefaultValue( 6 )]
		public int BrambleTicksPerDamage { get; set; } = 6;

		[Range( 1, 9999999 )]
		[DefaultValue( 20 )]
		public int BrambleDamage { get; set; } = 10;


		[Range( 1, 60 * 60 * 60 * 24 )]
		[DefaultValue( 60 * 30 )]
		public int BrambleWallTickDurationUntilErosionBegin { get; set; } = 60 * 30;

		[Range( 1, 60 * 60 * 60 * 24 )]
		[DefaultValue( 60 * 15 )]
		public int BrambleEnclosureTickDurationUntilErosionBegin { get; set; } = 60 * 15;


		[Range( 0f, 1f )]
		[DefaultValue( 0.2f )]
		public float BrambleStickiness { get; set; } = 0.2f;

		[Range( 1, 128 )]
		[DefaultValue( 4 )]
		public int BrambleThickness { get; set; } = 4;

		[Range( 0f, 1f )]
		[DefaultValue( 0.15f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float BrambleDensity { get; set; } = 0.15f;
	}
}
