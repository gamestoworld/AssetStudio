﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetStudio
{
    public class MeshRenderer
    {
        public PPtr m_GameObject;
        public bool m_Enabled;
        public byte m_CastShadows; //bool prior to version 5
        public bool m_ReceiveShadows;
        public ushort m_LightmapIndex;
        public ushort m_LightmapIndexDynamic;
        public PPtr[] m_Materials;

        protected MeshRenderer() { }

        public MeshRenderer(AssetPreloadData preloadData)
        {
            var sourceFile = preloadData.sourceFile;
            var reader = preloadData.InitReader();

            m_GameObject = sourceFile.ReadPPtr();

            if (sourceFile.version[0] < 5)
            {
                m_Enabled = reader.ReadBoolean();
                m_CastShadows = reader.ReadByte();
                m_ReceiveShadows = reader.ReadBoolean();
                m_LightmapIndex = reader.ReadByte();
            }
            else
            {
                m_Enabled = reader.ReadBoolean();
                reader.AlignStream(4);
                m_CastShadows = reader.ReadByte();
                m_ReceiveShadows = reader.ReadBoolean();
                reader.AlignStream(4);

                m_LightmapIndex = reader.ReadUInt16();
                m_LightmapIndexDynamic = reader.ReadUInt16();
            }

            if (sourceFile.version[0] >= 3) { reader.Position += 16; } //Vector4f m_LightmapTilingOffset
            if (sourceFile.version[0] >= 5) { reader.Position += 16; } //Vector4f m_LightmapTilingOffsetDynamic

            m_Materials = new PPtr[reader.ReadInt32()];
            for (int m = 0; m < m_Materials.Length; m++)
            {
                m_Materials[m] = sourceFile.ReadPPtr();
            }

        }
    }
}
