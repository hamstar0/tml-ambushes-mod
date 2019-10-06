﻿using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes {
	partial class AmbushesWorld : ModWorld {
		private static object MyLock = new object();



		////////////////

		private int MaxAmbushes;
		private int AmbushRegenDelay;

		private TilePattern AmbushAreaPattern;
		private IDictionary<int, IDictionary<int, Ambush>> Ambushes
			= new Dictionary<int, IDictionary<int, Ambush>>();



		////////////////

		public override void Initialize() {
			var mymod = (AmbushesMod)this.mod;

			this.AmbushAreaPattern = new TilePattern( new TilePatternBuilder {
				HasLava = false,
				IsSolid = false,
				IsPlatform = false,
				IsActuated = false,
			} );

			switch( WorldHelpers.GetSize() ) {
			default:
			case WorldSize.SubSmall:
				this.MaxAmbushes = mymod.Config.TinyWorldInitialAmbushes;
				break;
			case WorldSize.Small:
				this.MaxAmbushes = mymod.Config.SmallWorldInitialAmbushes;
				break;
			case WorldSize.Medium:
				this.MaxAmbushes = mymod.Config.MediumWorldInitialAmbushes;
				break;
			case WorldSize.Large:
				this.MaxAmbushes = mymod.Config.LargeWorldInitialAmbushes;
				break;
			case WorldSize.SuperLarge:
				this.MaxAmbushes = mymod.Config.HugeWorldInitialAmbushes;
				break;
			}

			this.InitializeAmbushes();
		}


		////////////////

		public override void PreUpdate() {
			if( Main.netMode != 1 ) {
				this.UpdateAmbushes();
			}
		}
	}
}
