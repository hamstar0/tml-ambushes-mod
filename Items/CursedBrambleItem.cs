using Ambushes.Tiles;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes.Items {
	class CursedBrambleItem : ModItem {
		public const int Width = 20;
		public const int Height = 20;


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Cursed Bramble" );
			this.Tooltip.SetDefault( "Ouch." );
		}

		public override void SetDefaults() {
			this.item.width = CursedBrambleItem.Width;
			this.item.height = CursedBrambleItem.Height;
			this.item.value = Item.buyPrice( 0, 0, 1, 0 );
			this.item.maxStack = 999;
			this.item.useTurn = true;
			this.item.autoReuse = true;
			this.item.useAnimation = 15;
			this.item.useTime = 10;
			this.item.useStyle = 1;
			this.item.consumable = true;
			this.item.createTile = ModContent.TileType<CursedBrambleTile>();
		}
	}
}
