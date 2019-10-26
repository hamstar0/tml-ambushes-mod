using Ambushes.Ambushes;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.TModLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace Ambushes {
	abstract partial class Ambush {
		protected static List<Ambush> StaticInstances;
		protected static float TotalWeight = 0f;



		////////////////

		internal static void OnModsLoad() {
			IEnumerable<Type> ambushTypes = ReflectionHelpers.GetAllAvailableSubTypesFromMods( typeof(Ambush) )
				.Where( t => !t.IsAbstract );

			Ambush.StaticInstances = ambushTypes.SafeSelect(
				ambushType => (Ambush)Activator.CreateInstance(ambushType, true)
			).SafeOrderBy( a => {
				Ambush.TotalWeight += a.WorldGenWeight;
				return a.WorldGenWeight;
			} ).ToList();
		}

		internal static void OnModsUnload() {
			Ambush.StaticInstances = null;
		}


		////////////////

		public static int GetAmbushCode( Ambush ambush ) {
			for( int i=0; i<Ambush.StaticInstances.Count; i++ ) {
				if( ambush.GetType().Name == Ambush.StaticInstances[i].GetType().Name ) {
					return i;
				}
			}
			return -1;
		}


		////////////////

		public static Ambush CreateRandomType( int tileX, int tileY ) {
			float rand = TmlHelpers.SafelyGetRand().NextFloat() * (float)Ambush.TotalWeight;
			Ambush randAmbush = null;

			float counted = 0f;
			foreach( Ambush ambush in Ambush.StaticInstances ) {
				if( rand >= counted && rand < ( ambush.WorldGenWeight + counted ) ) {
					randAmbush = ambush;
					break;
				}
				counted += ambush.WorldGenWeight;
			}
			
			return randAmbush?.CloneRandomized( tileX, tileY );
		}


		public static Ambush CreateType( int ambushType, int tileX, int tileY, bool isEntrapping ) {
			Ambush template = Ambush.StaticInstances[ ambushType ];
			Ambush ambush = template.CloneRandomized( tileX, tileY );

			if( ambush is BrambleEnclosureAmbush ) {
				var brambleAmbush = (BrambleEnclosureAmbush)ambush;
				brambleAmbush.IsEntrapping = isEntrapping;
			}

			return ambush;
		}
	}
}
