using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;


namespace Ambushes.Sounds {
	public class LowAmbushBGM : ModSound {
		public override SoundEffectInstance PlaySound( ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type ) {
			soundInstance = this.sound.CreateInstance();
			soundInstance.Volume = volume * 0.5f;
			soundInstance.Pan = pan;
			//soundInstance.Pitch = -1.0f;
			return soundInstance;
		}
	}
}
