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
		[DefaultValue( 128 )]
		public int TinyWorldInitialAmbushes { get; set; } = 160;    // SmallWorldInitialAmbushes / 2
		[DefaultValue( 192 )]
		public int SmallWorldInitialAmbushes { get; set; } = 256;  // 4200 x 1200 = 5040000
		[DefaultValue( 384 )]
		public int MediumWorldInitialAmbushes { get; set; } = 512; // 6400 x 1800 = 11520000
		[DefaultValue( 768 )]
		public int LargeWorldInitialAmbushes { get; set; } = 1024;  // 8400 x 2400 = 20160000
		[DefaultValue( 1280 )]
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

		[DefaultValue( 6 )]
		public int BrambleTicksPerDamage { get; set; } = 6;

		[DefaultValue( 1 )]
		public int BrambleDamage { get; set; } = 1;
	}
}
