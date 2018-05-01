/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.IO;

namespace Dolittle.Serialization.Protobuf
{
    /// <summary>
    /// Defines a serializer for serializing to and from protobuf binary format
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Get the length in bytes of an instance of a specific type
        /// </summary>
        /// <param name="instance">Instance to get length of</param>
        int GetLengthOf<T>(T instance);

        /// <summary>
        /// Serialize to protobuf into a <see cref="Stream"/> directly
        /// </summary>
        /// <param name="instance">Instance of object to serialize</param>
        /// <param name="stream"><see cref="Stream"/> to serialize into</param>
        /// <param name="includeLength">Whether or not to include length of the message prefixed or not</param>
        void ToProtobuf<T>(T instance, Stream stream, bool includeLength = false);

        /// <summary>
        /// Serialize to protobuf 
        /// </summary>
        /// <param name="instance">Instance of object to serialize</param>
        /// <param name="includeLength">Whether or not to include length of the message prefixed or not</param>
        /// <returns>The serialized message</returns>
        byte[] ToProtobuf<T>(T instance, bool includeLength = false);

        /// <summary>
        /// Deserialize from protobuf from a <see cref="Stream"/>
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> to deserialize from</param>
        /// <param name="includesLength">Indicates wether or not the message is prefixed with a length</param>
        /// <returns>An instance of the deserialized object</returns>
        T FromProtobuf<T>(Stream stream, bool includesLength = false);

        /// <summary>
        /// Deserialize from protobuf from a array of bytes
        /// </summary>
        /// <param name="bytes">Bytes to deserialize from</param>
        /// <param name="includesLength">Indicates wether or not the message is prefixed with a length</param>
        /// <returns></returns>
        T FromProtobuf<T>(byte[] bytes, bool includesLength = false);
    }
}