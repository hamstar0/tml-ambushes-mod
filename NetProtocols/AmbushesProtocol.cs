using Ambushes.Ambushes;
using HamstarHelpers.Classes.Protocols.Packet.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;


namespace Ambushes.NetProtocols {
	class AmbushesProtocol : PacketProtocolRequestToServer {
		public static void QuickRequest() {
			PacketProtocolRequestToServer.QuickRequestToServer<AmbushesProtocol>( -1 );
		}



		////////////////

		public int[] AmbushTypes;
		public bool[] IsEntrapments;
		public int[] TilePositionXs;
		public int[] TilePositionYs;



		////////////////

		private AmbushesProtocol() { }

		////////////////

		protected override void InitializeServerSendData( int who ) {
			var myworld = ModContent.GetInstance<AmbushesWorld>();
			IEnumerable<Ambush> ambushes = myworld.AmbushMngr.GetAllAmbushes();
			int count = ambushes.Count();

			this.AmbushTypes = new int[count];
			this.IsEntrapments = new bool[count];
			this.TilePositionXs = new int[count];
			this.TilePositionYs = new int[count];

			int i = 0;
			foreach( Ambush ambush in ambushes ) {
				var ab = ambush as BrambleEnclosureAmbush;

				this.AmbushTypes[i] = Ambush.GetAmbushCode( ambush );
				this.IsEntrapments[i] = ab?.IsEntrapping ?? false;
				this.TilePositionXs[i] = ambush.TileY;
				this.TilePositionYs[i] = ambush.TileY;
				i++;
			}
		}

		////////////////

		protected override void ReceiveReply() {
			var myworld = ModContent.GetInstance<AmbushesWorld>();
			Ambush ambush;

			for( int i=0; i<this.AmbushTypes.Length; i++ ) {
				ambush = Ambush.CreateType( this.AmbushTypes[i], this.TilePositionXs[i], this.TilePositionYs[i], this.IsEntrapments[i] );
				myworld.AmbushMngr.ArmAmbush( ambush );
			}
		}
	}
}
