using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace Ambushes {
	public class AmbushesConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;


		////

		[Header( "Debug settings" )]
		public bool DebugModeInfo { get; set; } = false;
		public bool DebugModeInfoMap { get; set; } = false;

		////

		[Header( "Ambush world settings" )]
		[DefaultValue( 160 )]
		public int TinyWorldInitialAmbushes { get; set; } = 160;    // SmallWorldInitialAmbushes / 2
		[DefaultValue( 256 )]
		public int SmallWorldInitialAmbushes { get; set; } = 256;  // 4200 x 1200 = 5040000
		[DefaultValue( 512 )]
		public int MediumWorldInitialAmbushes { get; set; } = 512; // 6400 x 1800 = 11520000
		[DefaultValue( 1024 )]
		public int LargeWorldInitialAmbushes { get; set; } = 1024;  // 8400 x 2400 = 20160000
		[DefaultValue( 2048 )]
		public int HugeWorldInitialAmbushes { get; set; } = 2048;

		[DefaultValue( 10 )]
		public int AmbushInitialGenerationSlowness { get; set; } = 10;

		[DefaultValue( 60 * 60 * 15 )]
		[Label( "Duration (in ticks) until a used ambush re-randomizes" )]
		public int AmbushRegenTickRate { get; set; } = 60 * 60 * 15;    // 15 minutes

		[DefaultValue( 64 )]
		public int MinimumAmbushTileSpacing { get; set; } = 64;

		////

		[Header( "Ambush settings" )]
		[DefaultValue( 8 )]
		public int AmbushTriggerRadiusTiles { get; set; } = 8;
	}
}
