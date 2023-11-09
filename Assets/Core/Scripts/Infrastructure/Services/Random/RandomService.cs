using IdGen;

namespace Services.Random
{
    public class RandomService : IRandomService
    {
        private IdGenerator _generator = new IdGenerator(0);
        
        public long GenerateId() => 
            _generator.CreateId();
    }
}