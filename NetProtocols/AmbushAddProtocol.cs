using Ambushes.Ambushes;
using HamstarHelpers.Classes.Protocols.Packet.Interfaces;
using Terraria.ModLoader;


namespace Ambushes.NetProtocols {
	class AmbushAddProtocol : PacketProtocolSendToClient {
		public static void QuickSendToClients( Ambush ambush ) {
			var protocol = new AmbushAddProtocol( ambush );
			protocol.SendToClient( -1, -1 );
		}



		////////////////

		public int AmbushType;
		public bool IsEntrapment;
		public int TilePositionX;
		public int TilePositionY;



		////////////////

		private AmbushAddProtocol() { }

		private AmbushAddProtocol( Ambush ambush ) {
			var ab = ambush as BrambleEnclosureAmbush;

			this.AmbushType = Ambush.GetAmbushCode( ambush );
			this.AmbushType = Ambush.GetAmbushCode( ambush );
			this.IsEntrapment = ab?.IsEntrapping ?? false;
			this.TilePositionX = ambush.TileY;
			this.TilePositionY = ambush.TileY;
		}

		////////////////

		protected override void InitializeServerSendData( int who ) {
		}


		////////////////

		protected override void Receive() {
			var myworld = ModContent.GetInstance<AmbushesWorld>();

			Ambush ambush = Ambush.CreateType( this.AmbushType, this.TilePositionX, this.TilePositionY, this.IsEntrapment );
			myworld.AmbushMngr.ArmAmbush( ambush );
		}
	}
}
