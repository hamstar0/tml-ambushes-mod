using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Ambushes.Tiles {
	public class CursedBrambleTile : ModTile {
		public override void SetDefaults() {
			//Main.tileSolid[this.Type] = true;
			//Main.tileMergeDirt[this.Type] = true;
			//Main.tileBlockLight[this.Type] = true;
			//Main.tileLighted[this.Type] = true;
			this.dustType = DustID.Granite;
			this.AddMapEntry( new Color( 128, 64, 128 ) );
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}