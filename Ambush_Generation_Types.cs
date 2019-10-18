using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using System;
using Terraria;


namespace Ambushes {
	abstract partial class Ambush {
		public static Ambush CreateRandomType( int tileX, int tileY ) {
			switch( TmlHelpers.SafelyGetRand().Next( 6+2 ) ) {
			default:
			case 0:
				return Ambush.CreateRandomFloodType( tileX, tileY );
			case 1:
				return Ambush.CreateRandomBrambleWallType( tileX, tileY );
			case 2:
				return Ambush.CreateRandomFlyerSwarmType( tileX, tileY );
			case 3:
				return Ambush.CreateRandomWormsInfestationType( tileX, tileY );
			case 4:
				return Ambush.CreateRandomSkeletonRaidersType( tileX, tileY );
			case 5:
				return Ambush.CreateRandomMinibossType( tileX, tileY );
			}
		}


		////////////////

		public static Ambush CreateRandomFloodType( int tileX, int tileY ) {
			bool isEntrapping = TmlHelpers.SafelyGetRand().Next( 4 ) == 0;
		}

		public static Ambush CreateRandomBrambleWallType ( int tileX, int tileY ) {
			bool isEntrapping = false;
		}

		public static Ambush CreateRandomFlyerSwarmType( int tileX, int tileY ) {
			bool isEntrapping = TmlHelpers.SafelyGetRand().Next( 4 ) == 0;
		}

		public static Ambush CreateRandomWormsInfestationType( int tileX, int tileY ) {

		}

		public static Ambush CreateRandomSkeletonRaidersType( int tileX, int tileY ) {
			bool isEntrapping = TmlHelpers.SafelyGetRand().Next( 4 ) == 0;

		}

		public static Ambush CreateRandomMinibossType( int tileX, int tileY ) {
			bool isEntrapping = TmlHelpers.SafelyGetRand().Next( 4 ) == 0;

		}
	}
}
