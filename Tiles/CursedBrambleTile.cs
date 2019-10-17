using HamstarHelpers.Helpers.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Ambushes.Tiles {
	public partial class CursedBrambleTile : ModTile {
		public override void SetDefaults() {
			//Main.tileSolid[this.Type] = true;
			//Main.tileMergeDirt[this.Type] = true;
			//Main.tileBlockLight[this.Type] = true;
			//Main.tileLighted[this.Type] = true;
			Main.tileNoAttach[this.Type] = true;
			Main.tileLavaDeath[this.Type] = false;
			this.dustType = DustID.Granite;
			this.AddMapEntry( new Color(128, 64, 128) );
		}

		public override void NumDust( int i, int j, bool fail, ref int num ) {
			num = fail ? 1 : 3;
		}


		public override void RandomUpdate( int i, int j ) {
			TileHelpers.KillTile( i, j, false, false );
		}
	}
}
