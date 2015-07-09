using System.Collections.Generic;

namespace CanmanSharp.Core
{
    public abstract class RegisterFactory<T> where T : class
    {
        private IDictionary<string, T> _registers;

        /// <summary>
        ///     # Registers a blender. Can be used to add your own blenders outside of
        ///     the core library, if needed.
        ///     @param [String] name Name of the blender.
        ///     @param [Function] func The blender function.
        /// </summary>
        /// <param name="BlenderName"></param>
        /// <param name="blender"></param>
        public void Register(string registerName, T registerObject)
        {
            if (_registers == null)
                _registers = new Dictionary<string, T>();
            if (_registers.ContainsKey(registerName))
                _registers[registerName] = registerObject;
            else
                _registers.Add(registerName, registerObject);
        }

        public T GetBlender(string registerName)
        {
            T registerObject;
            if (_registers.TryGetValue(registerName, out registerObject))
                return registerObject;
            return null;
        }
    }
}