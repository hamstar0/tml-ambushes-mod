using HamstarHelpers.Helpers.World;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Ambushes {
	partial class AmbushesWorld : ModWorld {
		private static object MyLock = new object();



		////////////////

		private int AmbushRegenDelay;

		////

		internal AmbushManager AmbushMngr { get; private set; }

		public int MaxAmbushes { get; private set; }



		////////////////

		public override void Initialize() {
			WorldSize size = WorldHelpers.GetSize();

			switch( size ) {
			default:
			case WorldSize.SubSmall:
				this.MaxAmbushes = AmbushesMod.Config.TinyWorldInitialAmbushes;
				break;
			case WorldSize.Small:
				this.MaxAmbushes = AmbushesMod.Config.SmallWorldInitialAmbushes;
				break;
			case WorldSize.Medium:
				this.MaxAmbushes = AmbushesMod.Config.MediumWorldInitialAmbushes;
				break;
			case WorldSize.Large:
				this.MaxAmbushes = AmbushesMod.Config.LargeWorldInitialAmbushes;
				break;
			case WorldSize.SuperLarge:
				this.MaxAmbushes = AmbushesMod.Config.HugeWorldInitialAmbushes;
				break;
			}

			this.AmbushMngr = new AmbushManager( size );
		}


		////////////////

		public override void Load( TagCompound tag ) {
			this.AmbushMngr.Load( tag );
		}

		public override TagCompound Save() {
			var tag = new TagCompound();
			this.AmbushMngr.Save( tag );
			return tag;
		}


		////////////////

		public override void PreUpdate() {
			if( Main.netMode != 1 ) {
				this.UpdateAmbushesRegen( this.MaxAmbushes );
				this.AmbushMngr.Update();
			}
		}
	}
}
