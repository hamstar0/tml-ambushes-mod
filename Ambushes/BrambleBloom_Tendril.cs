using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace Ambushes.Ambushes {
	class BrambleBloomTendril {
		public const int BaseSplitOdds = 24;



		////////////////

		private float InitialDegrees;

		private float CurrentTileX;
		private float CurrentTileY;
		private float CurrentDegrees;

		private int SplitOdds = BrambleBloomTendril.BaseSplitOdds;



		////////////////

		public BrambleBloomTendril( float degrees, int tileX, int tileY ) {
			this.InitialDegrees = degrees;
			this.CurrentDegrees = degrees;
			this.CurrentTileX = tileX;
			this.CurrentTileY = tileY;
		}

		////

		public BrambleBloomTendril CloneAsSplit() {
			float deg = this.InitialDegrees - this.CurrentDegrees;
			deg += this.InitialDegrees;

			var clone = new BrambleBloomTendril( this.InitialDegrees, (int)this.CurrentTileX, (int)this.CurrentTileY );
			clone.CurrentTileX = this.CurrentTileX;
			clone.CurrentTileY = this.CurrentTileY;
			clone.CurrentDegrees = deg;
			clone.SplitOdds += BrambleBloomTendril.BaseSplitOdds;
			this.SplitOdds += BrambleBloomTendril.BaseSplitOdds;

			return clone;
		}


		////////////////

		public bool IsSplit() {
			return TmlHelpers.SafelyGetRand().Next( this.SplitOdds ) == 0;
		}


		////////////////

		private Vector2 GetDirection() {
			float variance = (TmlHelpers.SafelyGetRand().NextFloat() * 180f) - 90f;

			this.CurrentDegrees = this.CurrentDegrees + variance;
			this.CurrentDegrees += this.InitialDegrees;
			this.CurrentDegrees *= 0.5f;

			float radians = MathHelper.ToRadians( this.CurrentDegrees );

			return new Vector2( (float)Math.Cos(radians), (float)Math.Sin(radians) );
		}


		////////////////

		public (int TileX, int TileY) Grow() {
			Vector2 dir = this.GetDirection();
			this.CurrentTileX += dir.X;
			this.CurrentTileY += dir.Y;

			int x = (int)this.CurrentTileX;
			int y = (int)this.CurrentTileY;

			return ( (int)this.CurrentTileX, (int)this.CurrentTileY);
		}
	}
}
