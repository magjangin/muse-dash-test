using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppSpine.Unity.AttachmentTools;

public static class AtlasUtilities : Il2CppSystem.Object
{
	public sealed class IntAndAtlasRegionKey : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_i;

		private static readonly System.IntPtr NativeFieldInfoPtr_region;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_AtlasRegion_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0;

		public unsafe int i
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_i);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_i)) = num;
			}
		}

		public unsafe AtlasRegion region
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_region);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AtlasRegion>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_region)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)atlasRegion));
			}
		}

		static IntAndAtlasRegionKey()
		{
			Il2CppClassPointerStore<IntAndAtlasRegionKey>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, "IntAndAtlasRegionKey");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<IntAndAtlasRegionKey>.NativeClassPtr);
			NativeFieldInfoPtr_i = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IntAndAtlasRegionKey>.NativeClassPtr, "i");
			NativeFieldInfoPtr_region = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IntAndAtlasRegionKey>.NativeClassPtr, "region");
			NativeMethodInfoPtr__ctor_Public_Void_Int32_AtlasRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntAndAtlasRegionKey>.NativeClassPtr, 100664438);
			NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntAndAtlasRegionKey>.NativeClassPtr, 100664439);
		}

		[CallerCount(11)]
		[CachedScanResults(RefRangeStart = 475054, RefRangeEnd = 475065, XrefRangeStart = 475054, XrefRangeEnd = 475065, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe IntAndAtlasRegionKey(int i, AtlasRegion region)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<IntAndAtlasRegionKey>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = (nint)(&i);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)region);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_AtlasRegion_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793494, XrefRangeEnd = 793495, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe override int GetHashCode()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public IntAndAtlasRegionKey(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public IntAndAtlasRegionKey()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<IntAndAtlasRegionKey>.NativeClassPtr))
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_SpineTextureFormat;

	private static readonly System.IntPtr NativeFieldInfoPtr_DefaultMipmapBias;

	private static readonly System.IntPtr NativeFieldInfoPtr_UseMipMaps;

	private static readonly System.IntPtr NativeFieldInfoPtr_DefaultScale;

	private static readonly System.IntPtr NativeFieldInfoPtr_NonrenderingRegion;

	private static readonly System.IntPtr NativeFieldInfoPtr_CachedRegionTextures;

	private static readonly System.IntPtr NativeFieldInfoPtr_CachedRegionTexturesList;

	private static readonly System.IntPtr NativeMethodInfoPtr_Init_Private_Static_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToAtlasRegion_Public_Static_AtlasRegion_Texture2D_Material_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToAtlasRegion_Public_Static_AtlasRegion_Texture2D_Shader_Single_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToAtlasRegionPMAClone_Public_Static_AtlasRegion_Texture2D_Material_TextureFormat_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToAtlasRegionPMAClone_Public_Static_AtlasRegion_Texture2D_Shader_TextureFormat_Boolean_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToSpineAtlasPage_Public_Static_AtlasPage_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToAtlasRegion_Public_Static_AtlasRegion_Sprite_AtlasPage_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToAtlasRegion_Public_Static_AtlasRegion_Sprite_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToAtlasRegionPMAClone_Public_Static_AtlasRegion_Sprite_Material_TextureFormat_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToAtlasRegionPMAClone_Public_Static_AtlasRegion_Sprite_Shader_TextureFormat_Boolean_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToAtlasRegion_Internal_Static_AtlasRegion_Sprite_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetRepackedAttachments_Public_Static_Void_List_1_Attachment_List_1_Attachment_Material_byref_Material_byref_Texture2D_Int32_Int32_TextureFormat_Boolean_String_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetRepackedSkin_Public_Static_Skin_Skin_String_Material_byref_Material_byref_Texture2D_Int32_Int32_TextureFormat_Boolean_Boolean_Boolean_Il2CppStructArray_1_Int32_Il2CppReferenceArray_1_Texture2D_Il2CppStructArray_1_TextureFormat_Il2CppStructArray_1_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetRepackedSkin_Public_Static_Skin_Skin_String_Shader_byref_Material_byref_Texture2D_Int32_Int32_TextureFormat_Boolean_Material_Boolean_Boolean_Il2CppStructArray_1_Int32_Il2CppReferenceArray_1_Texture2D_Il2CppStructArray_1_TextureFormat_Il2CppStructArray_1_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToSprite_Public_Static_Sprite_AtlasRegion_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClearCache_Public_Static_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToTexture_Public_Static_Texture2D_AtlasRegion_TextureFormat_Boolean_Int32_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToTexture_Private_Static_Texture2D_Sprite_TextureFormat_Boolean_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetClone_Private_Static_Texture2D_Texture2D_TextureFormat_Boolean_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CopyTexture_Private_Static_Void_Texture2D_Rect_Texture2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CopyTextureApplyPMA_Private_Static_Void_Texture2D_Rect_Texture2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsRenderable_Private_Static_Boolean_Attachment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SpineUnityFlipRect_Private_Static_Rect_Rect_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetUnityRect_Private_Static_Rect_AtlasRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetUnityRect_Private_Static_Rect_AtlasRegion_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetSpineAtlasRect_Private_Static_Rect_AtlasRegion_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UVRectToTextureRect_Private_Static_Rect_Rect_Int32_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TextureRectToUVRect_Private_Static_Rect_Rect_Int32_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UVRectToAtlasRegion_Private_Static_AtlasRegion_Rect_AtlasRegion_AtlasPage_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMainTexture_Private_Static_Texture2D_AtlasRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetTexture_Private_Static_Texture2D_AtlasRegion_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetTexture_Private_Static_Texture2D_AtlasRegion_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CopyTextureAttributesFrom_Private_Static_Void_Texture2D_Texture2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InverseLerp_Private_Static_Single_Single_Single_Single_0;

	public unsafe static TextureFormat SpineTextureFormat
	{
		get
		{
			Unsafe.SkipInit(out TextureFormat result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SpineTextureFormat, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SpineTextureFormat, (void*)(&textureFormat));
		}
	}

	public unsafe static float DefaultMipmapBias
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DefaultMipmapBias, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DefaultMipmapBias, (void*)(&num));
		}
	}

	public unsafe static bool UseMipMaps
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UseMipMaps, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UseMipMaps, (void*)(&flag));
		}
	}

	public unsafe static float DefaultScale
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DefaultScale, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DefaultScale, (void*)(&num));
		}
	}

	public unsafe static int NonrenderingRegion
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_NonrenderingRegion, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_NonrenderingRegion, (void*)(&num));
		}
	}

	public unsafe static Dictionary<IntAndAtlasRegionKey, Texture2D> CachedRegionTextures
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CachedRegionTextures, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<IntAndAtlasRegionKey, Texture2D>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CachedRegionTextures, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe static List<Texture2D> CachedRegionTexturesList
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CachedRegionTexturesList, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Texture2D>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CachedRegionTexturesList, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static AtlasUtilities()
	{
		Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity.AttachmentTools", "AtlasUtilities");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr);
		NativeFieldInfoPtr_SpineTextureFormat = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, "SpineTextureFormat");
		NativeFieldInfoPtr_DefaultMipmapBias = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, "DefaultMipmapBias");
		NativeFieldInfoPtr_UseMipMaps = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, "UseMipMaps");
		NativeFieldInfoPtr_DefaultScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, "DefaultScale");
		NativeFieldInfoPtr_NonrenderingRegion = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, "NonrenderingRegion");
		NativeFieldInfoPtr_CachedRegionTextures = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, "CachedRegionTextures");
		NativeFieldInfoPtr_CachedRegionTexturesList = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, "CachedRegionTexturesList");
		NativeMethodInfoPtr_Init_Private_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664403);
		NativeMethodInfoPtr_ToAtlasRegion_Public_Static_AtlasRegion_Texture2D_Material_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664404);
		NativeMethodInfoPtr_ToAtlasRegion_Public_Static_AtlasRegion_Texture2D_Shader_Single_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664405);
		NativeMethodInfoPtr_ToAtlasRegionPMAClone_Public_Static_AtlasRegion_Texture2D_Material_TextureFormat_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664406);
		NativeMethodInfoPtr_ToAtlasRegionPMAClone_Public_Static_AtlasRegion_Texture2D_Shader_TextureFormat_Boolean_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664407);
		NativeMethodInfoPtr_ToSpineAtlasPage_Public_Static_AtlasPage_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664408);
		NativeMethodInfoPtr_ToAtlasRegion_Public_Static_AtlasRegion_Sprite_AtlasPage_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664409);
		NativeMethodInfoPtr_ToAtlasRegion_Public_Static_AtlasRegion_Sprite_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664410);
		NativeMethodInfoPtr_ToAtlasRegionPMAClone_Public_Static_AtlasRegion_Sprite_Material_TextureFormat_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664411);
		NativeMethodInfoPtr_ToAtlasRegionPMAClone_Public_Static_AtlasRegion_Sprite_Shader_TextureFormat_Boolean_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664412);
		NativeMethodInfoPtr_ToAtlasRegion_Internal_Static_AtlasRegion_Sprite_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664413);
		NativeMethodInfoPtr_GetRepackedAttachments_Public_Static_Void_List_1_Attachment_List_1_Attachment_Material_byref_Material_byref_Texture2D_Int32_Int32_TextureFormat_Boolean_String_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664414);
		NativeMethodInfoPtr_GetRepackedSkin_Public_Static_Skin_Skin_String_Material_byref_Material_byref_Texture2D_Int32_Int32_TextureFormat_Boolean_Boolean_Boolean_Il2CppStructArray_1_Int32_Il2CppReferenceArray_1_Texture2D_Il2CppStructArray_1_TextureFormat_Il2CppStructArray_1_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664415);
		NativeMethodInfoPtr_GetRepackedSkin_Public_Static_Skin_Skin_String_Shader_byref_Material_byref_Texture2D_Int32_Int32_TextureFormat_Boolean_Material_Boolean_Boolean_Il2CppStructArray_1_Int32_Il2CppReferenceArray_1_Texture2D_Il2CppStructArray_1_TextureFormat_Il2CppStructArray_1_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664416);
		NativeMethodInfoPtr_ToSprite_Public_Static_Sprite_AtlasRegion_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664417);
		NativeMethodInfoPtr_ClearCache_Public_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664418);
		NativeMethodInfoPtr_ToTexture_Public_Static_Texture2D_AtlasRegion_TextureFormat_Boolean_Int32_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664419);
		NativeMethodInfoPtr_ToTexture_Private_Static_Texture2D_Sprite_TextureFormat_Boolean_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664420);
		NativeMethodInfoPtr_GetClone_Private_Static_Texture2D_Texture2D_TextureFormat_Boolean_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664421);
		NativeMethodInfoPtr_CopyTexture_Private_Static_Void_Texture2D_Rect_Texture2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664422);
		NativeMethodInfoPtr_CopyTextureApplyPMA_Private_Static_Void_Texture2D_Rect_Texture2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664423);
		NativeMethodInfoPtr_IsRenderable_Private_Static_Boolean_Attachment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664424);
		NativeMethodInfoPtr_SpineUnityFlipRect_Private_Static_Rect_Rect_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664425);
		NativeMethodInfoPtr_GetUnityRect_Private_Static_Rect_AtlasRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664426);
		NativeMethodInfoPtr_GetUnityRect_Private_Static_Rect_AtlasRegion_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664427);
		NativeMethodInfoPtr_GetSpineAtlasRect_Private_Static_Rect_AtlasRegion_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664428);
		NativeMethodInfoPtr_UVRectToTextureRect_Private_Static_Rect_Rect_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664429);
		NativeMethodInfoPtr_TextureRectToUVRect_Private_Static_Rect_Rect_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664430);
		NativeMethodInfoPtr_UVRectToAtlasRegion_Private_Static_AtlasRegion_Rect_AtlasRegion_AtlasPage_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664431);
		NativeMethodInfoPtr_GetMainTexture_Private_Static_Texture2D_AtlasRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664432);
		NativeMethodInfoPtr_GetTexture_Private_Static_Texture2D_AtlasRegion_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664433);
		NativeMethodInfoPtr_GetTexture_Private_Static_Texture2D_AtlasRegion_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664434);
		NativeMethodInfoPtr_CopyTextureAttributesFrom_Private_Static_Void_Texture2D_Texture2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664435);
		NativeMethodInfoPtr_InverseLerp_Private_Static_Single_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasUtilities>.NativeClassPtr, 100664436);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793495, XrefRangeEnd = 793499, MetadataInitTokenRva = 47238024L, MetadataInitFlagRva = 59827093L)]
	public unsafe static void Init()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Init_Private_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793499, XrefRangeEnd = 793504, MetadataInitTokenRva = 47238424L, MetadataInitFlagRva = 59827094L)]
	public unsafe static AtlasRegion ToAtlasRegion(this Texture2D t, Material materialPropertySource, float scale = 0.01f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)t);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialPropertySource);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &scale;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToAtlasRegion_Public_Static_AtlasRegion_Texture2D_Material_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AtlasRegion>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 793529, RefRangeEnd = 793531, XrefRangeStart = 793504, XrefRangeEnd = 793529, MetadataInitTokenRva = 47238356L, MetadataInitFlagRva = 59827095L)]
	public unsafe static AtlasRegion ToAtlasRegion(this Texture2D t, Shader shader, float scale = 0.01f, Material materialPropertySource = null)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)t);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)shader);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &scale;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialPropertySource);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToAtlasRegion_Public_Static_AtlasRegion_Texture2D_Shader_Single_Material_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AtlasRegion>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793531, XrefRangeEnd = 793536, MetadataInitTokenRva = 47238272L, MetadataInitFlagRva = 59827096L)]
	public unsafe static AtlasRegion ToAtlasRegionPMAClone(this Texture2D t, Material materialPropertySource, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)t);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialPropertySource);
		*(TextureFormat**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &textureFormat;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &mipmaps;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToAtlasRegionPMAClone_Public_Static_AtlasRegion_Texture2D_Material_TextureFormat_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AtlasRegion>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 793561, RefRangeEnd = 793562, XrefRangeStart = 793536, XrefRangeEnd = 793561, MetadataInitTokenRva = 47238192L, MetadataInitFlagRva = 59827097L)]
	public unsafe static AtlasRegion ToAtlasRegionPMAClone(this Texture2D t, Shader shader, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, Material materialPropertySource = null)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)t);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)shader);
		*(TextureFormat**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &textureFormat;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &mipmaps;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialPropertySource);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToAtlasRegionPMAClone_Public_Static_AtlasRegion_Texture2D_Shader_TextureFormat_Boolean_Material_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AtlasRegion>(intPtr) : null;
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 793573, RefRangeEnd = 793579, XrefRangeStart = 793562, XrefRangeEnd = 793573, MetadataInitTokenRva = 47238500L, MetadataInitFlagRva = 59827098L)]
	public unsafe static AtlasPage ToSpineAtlasPage(this Material m)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)m);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToSpineAtlasPage_Public_Static_AtlasPage_Material_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AtlasPage>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793579, XrefRangeEnd = 793584, MetadataInitTokenRva = 47238324L, MetadataInitFlagRva = 59827099L)]
	public unsafe static AtlasRegion ToAtlasRegion(this Sprite s, AtlasPage page)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)s);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)page);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToAtlasRegion_Public_Static_AtlasRegion_Sprite_AtlasPage_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AtlasRegion>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793584, XrefRangeEnd = 793590, MetadataInitTokenRva = 47238456L, MetadataInitFlagRva = 59827100L)]
	public unsafe static AtlasRegion ToAtlasRegion(this Sprite s, Material material)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)s);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToAtlasRegion_Public_Static_AtlasRegion_Sprite_Material_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AtlasRegion>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793590, XrefRangeEnd = 793595, MetadataInitTokenRva = 47238236L, MetadataInitFlagRva = 59827101L)]
	public unsafe static AtlasRegion ToAtlasRegionPMAClone(this Sprite s, Material materialPropertySource, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)s);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialPropertySource);
		*(TextureFormat**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &textureFormat;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &mipmaps;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToAtlasRegionPMAClone_Public_Static_AtlasRegion_Sprite_Material_TextureFormat_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AtlasRegion>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 793620, RefRangeEnd = 793621, XrefRangeStart = 793595, XrefRangeEnd = 793620, MetadataInitTokenRva = 47238156L, MetadataInitFlagRva = 59827102L)]
	public unsafe static AtlasRegion ToAtlasRegionPMAClone(this Sprite s, Shader shader, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, Material materialPropertySource = null)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)s);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)shader);
		*(TextureFormat**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &textureFormat;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &mipmaps;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialPropertySource);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToAtlasRegionPMAClone_Public_Static_AtlasRegion_Sprite_Shader_TextureFormat_Boolean_Material_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AtlasRegion>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 793676, RefRangeEnd = 793679, XrefRangeStart = 793621, XrefRangeEnd = 793676, MetadataInitTokenRva = 47238408L, MetadataInitFlagRva = 59827103L)]
	public unsafe static AtlasRegion ToAtlasRegion(this Sprite s, bool isolatedTexture = false)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)s);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &isolatedTexture;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToAtlasRegion_Internal_Static_AtlasRegion_Sprite_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AtlasRegion>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793679, XrefRangeEnd = 793788, MetadataInitTokenRva = 47237768L, MetadataInitFlagRva = 59827104L)]
	public unsafe static void GetRepackedAttachments(List<Attachment> sourceAttachments, List<Attachment> outputAttachments, Material materialPropertySource, out Material outputMaterial, out Texture2D outputTexture, int maxAtlasSize = 1024, int padding = 2, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, string newAssetName = "Repacked Attachments", bool clearCache = false, bool useOriginalNonrenderables = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[12];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sourceAttachments);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)outputAttachments);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialPropertySource);
		byte* num = (byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		byte* num3 = (byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num4 = 0;
		*(nint**)num3 = &num4;
		*(int**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &maxAtlasSize;
		*(int**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &padding;
		*(TextureFormat**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = &textureFormat;
		*(bool**)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(System.IntPtr)))) = &mipmaps;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)9u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(newAssetName);
		*(bool**)((byte*)ptr + checked((nuint)10u * unchecked((nuint)sizeof(System.IntPtr)))) = &clearCache;
		*(bool**)((byte*)ptr + checked((nuint)11u * unchecked((nuint)sizeof(System.IntPtr)))) = &useOriginalNonrenderables;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetRepackedAttachments_Public_Static_Void_List_1_Attachment_List_1_Attachment_Material_byref_Material_byref_Texture2D_Int32_Int32_TextureFormat_Boolean_String_Boolean_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num5 = num2;
		outputMaterial = ((num5 == 0) ? null : new Material(num5));
		nint num6 = num4;
		outputTexture = ((num6 == 0) ? null : new Texture2D(num6));
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793788, XrefRangeEnd = 793793, MetadataInitTokenRva = 47237820L, MetadataInitFlagRva = 59827105L)]
	public unsafe static Skin GetRepackedSkin(this Skin o, string newName, Material materialPropertySource, out Material outputMaterial, out Texture2D outputTexture, int maxAtlasSize = 1024, int padding = 2, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, bool useOriginalNonrenderables = true, bool clearCache = false, Il2CppStructArray<int> additionalTexturePropertyIDsToCopy = null, Il2CppReferenceArray<Texture2D> additionalOutputTextures = null, Il2CppStructArray<TextureFormat> additionalTextureFormats = null, Il2CppStructArray<bool> additionalTextureIsLinear = null)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[15];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)o);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(newName);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialPropertySource);
		byte* num = (byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		byte* num3 = (byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num4 = 0;
		*(nint**)num3 = &num4;
		*(int**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &maxAtlasSize;
		*(int**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &padding;
		*(TextureFormat**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = &textureFormat;
		*(bool**)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(System.IntPtr)))) = &mipmaps;
		*(bool**)((byte*)ptr + checked((nuint)9u * unchecked((nuint)sizeof(System.IntPtr)))) = &useOriginalNonrenderables;
		*(bool**)((byte*)ptr + checked((nuint)10u * unchecked((nuint)sizeof(System.IntPtr)))) = &clearCache;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)11u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)additionalTexturePropertyIDsToCopy);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)12u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)additionalOutputTextures);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)13u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)additionalTextureFormats);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)14u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)additionalTextureIsLinear);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetRepackedSkin_Public_Static_Skin_Skin_String_Material_byref_Material_byref_Texture2D_Int32_Int32_TextureFormat_Boolean_Boolean_Boolean_Il2CppStructArray_1_Int32_Il2CppReferenceArray_1_Texture2D_Il2CppStructArray_1_TextureFormat_Il2CppStructArray_1_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num5 = num2;
		outputMaterial = ((num5 == 0) ? null : new Material(num5));
		nint num6 = num4;
		outputTexture = ((num6 == 0) ? null : new Texture2D(num6));
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Skin>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 793933, RefRangeEnd = 793934, XrefRangeStart = 793793, XrefRangeEnd = 793933, MetadataInitTokenRva = 47237788L, MetadataInitFlagRva = 59827106L)]
	public unsafe static Skin GetRepackedSkin(this Skin o, string newName, Shader shader, out Material outputMaterial, out Texture2D outputTexture, int maxAtlasSize = 1024, int padding = 2, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, Material materialPropertySource = null, bool clearCache = false, bool useOriginalNonrenderables = true, Il2CppStructArray<int> additionalTexturePropertyIDsToCopy = null, Il2CppReferenceArray<Texture2D> additionalOutputTextures = null, Il2CppStructArray<TextureFormat> additionalTextureFormats = null, Il2CppStructArray<bool> additionalTextureIsLinear = null)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[16];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)o);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(newName);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)shader);
		byte* num = (byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		byte* num3 = (byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num4 = 0;
		*(nint**)num3 = &num4;
		*(int**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &maxAtlasSize;
		*(int**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &padding;
		*(TextureFormat**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = &textureFormat;
		*(bool**)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(System.IntPtr)))) = &mipmaps;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)9u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialPropertySource);
		*(bool**)((byte*)ptr + checked((nuint)10u * unchecked((nuint)sizeof(System.IntPtr)))) = &clearCache;
		*(bool**)((byte*)ptr + checked((nuint)11u * unchecked((nuint)sizeof(System.IntPtr)))) = &useOriginalNonrenderables;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)12u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)additionalTexturePropertyIDsToCopy);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)13u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)additionalOutputTextures);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)14u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)additionalTextureFormats);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)15u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)additionalTextureIsLinear);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetRepackedSkin_Public_Static_Skin_Skin_String_Shader_byref_Material_byref_Texture2D_Int32_Int32_TextureFormat_Boolean_Material_Boolean_Boolean_Il2CppStructArray_1_Int32_Il2CppReferenceArray_1_Texture2D_Il2CppStructArray_1_TextureFormat_Il2CppStructArray_1_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num5 = num2;
		outputMaterial = ((num5 == 0) ? null : new Material(num5));
		nint num6 = num4;
		outputTexture = ((num6 == 0) ? null : new Texture2D(num6));
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Skin>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793934, XrefRangeEnd = 793941, MetadataInitTokenRva = 47238556L, MetadataInitFlagRva = 59827107L)]
	public unsafe static Sprite ToSprite(this AtlasRegion ar, float pixelsPerUnit = 100f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)ar);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &pixelsPerUnit;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToSprite_Public_Static_Sprite_AtlasRegion_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Sprite>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 793964, RefRangeEnd = 793967, XrefRangeStart = 793941, XrefRangeEnd = 793964, MetadataInitTokenRva = 47237604L, MetadataInitFlagRva = 59827108L)]
	public unsafe static void ClearCache()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearCache_Public_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 794021, RefRangeEnd = 794024, XrefRangeStart = 793967, XrefRangeEnd = 794021, MetadataInitTokenRva = 47238628L, MetadataInitFlagRva = 59827109L)]
	public unsafe static Texture2D ToTexture(this AtlasRegion ar, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, int texturePropertyId = 0, bool linear = false, bool applyPMA = false)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)ar);
		*(TextureFormat**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &textureFormat;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &mipmaps;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &texturePropertyId;
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &linear;
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &applyPMA;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToTexture_Public_Static_Texture2D_AtlasRegion_TextureFormat_Boolean_Int32_Boolean_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 794071, RefRangeEnd = 794072, XrefRangeStart = 794024, XrefRangeEnd = 794071, MetadataInitTokenRva = 47238600L, MetadataInitFlagRva = 59827110L)]
	public unsafe static Texture2D ToTexture(this Sprite s, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, bool linear = false, bool applyPMA = false)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)s);
		*(TextureFormat**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &textureFormat;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &mipmaps;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &linear;
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &applyPMA;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToTexture_Private_Static_Texture2D_Sprite_TextureFormat_Boolean_Boolean_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 794095, RefRangeEnd = 794096, XrefRangeStart = 794072, XrefRangeEnd = 794095, MetadataInitTokenRva = 47237672L, MetadataInitFlagRva = 59827111L)]
	public unsafe static Texture2D GetClone(this Texture2D t, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, bool linear = false, bool applyPMA = false)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)t);
		*(TextureFormat**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &textureFormat;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &mipmaps;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &linear;
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &applyPMA;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetClone_Private_Static_Texture2D_Texture2D_TextureFormat_Boolean_Boolean_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 794105, RefRangeEnd = 794108, XrefRangeStart = 794096, XrefRangeEnd = 794105, MetadataInitTokenRva = 47237632L, MetadataInitFlagRva = 59827112L)]
	public unsafe static void CopyTexture(Texture2D source, Rect sourceRect, Texture2D destination)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		*(Rect**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &sourceRect;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)destination);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CopyTexture_Private_Static_Void_Texture2D_Rect_Texture2D_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 794115, RefRangeEnd = 794118, XrefRangeStart = 794108, XrefRangeEnd = 794115, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void CopyTextureApplyPMA(Texture2D source, Rect sourceRect, Texture2D destination)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		*(Rect**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &sourceRect;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)destination);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CopyTextureApplyPMA_Private_Static_Void_Texture2D_Rect_Texture2D_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 794121, RefRangeEnd = 794125, XrefRangeStart = 794118, XrefRangeEnd = 794121, MetadataInitTokenRva = 47238056L, MetadataInitFlagRva = 59827113L)]
	public unsafe static bool IsRenderable(Attachment a)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)a);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsRenderable_Private_Static_Boolean_Attachment_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 794125, XrefRangeEnd = 794128, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Rect SpineUnityFlipRect(this Rect rect, int textureHeight)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&rect);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &textureHeight;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SpineUnityFlipRect_Private_Static_Rect_Rect_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 794136, RefRangeEnd = 794138, XrefRangeStart = 794128, XrefRangeEnd = 794136, MetadataInitTokenRva = 47237996L, MetadataInitFlagRva = 59827114L)]
	public unsafe static Rect GetUnityRect(this AtlasRegion region)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)region);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetUnityRect_Private_Static_Rect_AtlasRegion_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 794138, XrefRangeEnd = 794146, MetadataInitTokenRva = 47237952L, MetadataInitFlagRva = 59827115L)]
	public unsafe static Rect GetUnityRect(this AtlasRegion region, int textureHeight)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)region);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &textureHeight;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetUnityRect_Private_Static_Rect_AtlasRegion_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 794146, XrefRangeEnd = 794147, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Rect GetSpineAtlasRect(this AtlasRegion region, bool includeRotate = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)region);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &includeRotate;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetSpineAtlasRect_Private_Static_Rect_AtlasRegion_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 794147, XrefRangeEnd = 794155, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Rect UVRectToTextureRect(Rect uvRect, int texWidth, int texHeight)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&uvRect);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &texWidth;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &texHeight;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UVRectToTextureRect_Private_Static_Rect_Rect_Int32_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 794155, XrefRangeEnd = 794170, MetadataInitTokenRva = 47238104L, MetadataInitFlagRva = 59827116L)]
	public unsafe static Rect TextureRectToUVRect(Rect textureRect, int texWidth, int texHeight)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&textureRect);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &texWidth;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &texHeight;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TextureRectToUVRect_Private_Static_Rect_Rect_Int32_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 794206, RefRangeEnd = 794208, XrefRangeStart = 794170, XrefRangeEnd = 794206, MetadataInitTokenRva = 47238692L, MetadataInitFlagRva = 59827117L)]
	public unsafe static AtlasRegion UVRectToAtlasRegion(Rect uvRect, AtlasRegion referenceRegion, AtlasPage page)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&uvRect);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)referenceRegion);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)page);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UVRectToAtlasRegion_Private_Static_AtlasRegion_Rect_AtlasRegion_AtlasPage_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AtlasRegion>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 794214, RefRangeEnd = 794216, XrefRangeStart = 794208, XrefRangeEnd = 794214, MetadataInitTokenRva = 47237708L, MetadataInitFlagRva = 59827118L)]
	public unsafe static Texture2D GetMainTexture(this AtlasRegion region)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)region);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMainTexture_Private_Static_Texture2D_AtlasRegion_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 794216, XrefRangeEnd = 794222, MetadataInitTokenRva = 47237872L, MetadataInitFlagRva = 59827119L)]
	public unsafe static Texture2D GetTexture(this AtlasRegion region, string texturePropertyName)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)region);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(texturePropertyName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetTexture_Private_Static_Texture2D_AtlasRegion_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 794222, XrefRangeEnd = 794228, MetadataInitTokenRva = 47237904L, MetadataInitFlagRva = 59827120L)]
	public unsafe static Texture2D GetTexture(this AtlasRegion region, int texturePropertyId)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)region);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &texturePropertyId;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetTexture_Private_Static_Texture2D_AtlasRegion_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 794239, RefRangeEnd = 794241, XrefRangeStart = 794228, XrefRangeEnd = 794239, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void CopyTextureAttributesFrom(this Texture2D destination, Texture2D source)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)destination);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CopyTextureAttributesFrom_Private_Static_Void_Texture2D_Texture2D_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe static float InverseLerp(float a, float b, float value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&a);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &value;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InverseLerp_Private_Static_Single_Single_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public AtlasUtilities(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
