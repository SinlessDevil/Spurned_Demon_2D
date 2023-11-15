namespace AudioService
{
    public interface IAudioClipsService
    {
        Sound[] Sounds { get; set; }

        public void PlayClip(TypeSound typeSound);
        public void StopClip(TypeSound typeSound);
    }
}