using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine;

public class AtlasPage : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_name;

	private static readonly System.IntPtr NativeFieldInfoPtr_format;

	private static readonly System.IntPtr NativeFieldInfoPtr_minFilter;

	private static readonly System.IntPtr NativeFieldInfoPtr_magFilter;

	private static readonly System.IntPtr NativeFieldInfoPtr_uWrap;

	private static readonly System.IntPtr NativeFieldInfoPtr_vWrap;

	private static readonly System.IntPtr NativeFieldInfoPtr_rendererObject;

	private static readonly System.IntPtr NativeFieldInfoPtr_width;

	private static readonly System.IntPtr NativeFieldInfoPtr_height;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clone_Public_AtlasPage_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe string name
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_name);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_name)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Format format
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_format);
			return *(Format*)num;
		}
		set
		{
			*(Format*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_format)) = format;
		}
	}

	public unsafe TextureFilter minFilter
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_minFilter);
			return *(TextureFilter*)num;
		}
		set
		{
			*(TextureFilter*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_minFilter)) = textureFilter;
		}
	}

	public unsafe TextureFilter magFilter
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_magFilter);
			return *(TextureFilter*)num;
		}
		set
		{
			*(TextureFilter*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_magFilter)) = textureFilter;
		}
	}

	public unsafe TextureWrap uWrap
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uWrap);
			return *(TextureWrap*)num;
		}
		set
		{
			*(TextureWrap*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uWrap)) = textureWrap;
		}
	}

	public unsafe TextureWrap vWrap
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vWrap);
			return *(TextureWrap*)num;
		}
		set
		{
			*(TextureWrap*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vWrap)) = textureWrap;
		}
	}

	public unsafe Il2CppSystem.Object rendererObject
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rendererObject);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rendererObject)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj));
		}
	}

	public unsafe int width
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_width);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_width)) = num;
		}
	}

	public unsafe int height
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_height);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_height)) = num;
		}
	}

	static AtlasPage()
	{
		Il2CppClassPointerStore<AtlasPage>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "AtlasPage");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AtlasPage>.NativeClassPtr);
		NativeFieldInfoPtr_name = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasPage>.NativeClassPtr, "name");
		NativeFieldInfoPtr_format = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasPage>.NativeClassPtr, "format");
		NativeFieldInfoPtr_minFilter = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasPage>.NativeClassPtr, "minFilter");
		NativeFieldInfoPtr_magFilter = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasPage>.NativeClassPtr, "magFilter");
		NativeFieldInfoPtr_uWrap = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasPage>.NativeClassPtr, "uWrap");
		NativeFieldInfoPtr_vWrap = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasPage>.NativeClassPtr, "vWrap");
		NativeFieldInfoPtr_rendererObject = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasPage>.NativeClassPtr, "rendererObject");
		NativeFieldInfoPtr_width = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasPage>.NativeClassPtr, "width");
		NativeFieldInfoPtr_height = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasPage>.NativeClassPtr, "height");
		NativeMethodInfoPtr_Clone_Public_AtlasPage_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasPage>.NativeClassPtr, 100663505);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasPage>.NativeClassPtr, 100663506);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 783323, RefRangeEnd = 783324, XrefRangeStart = 783321, XrefRangeEnd = 783323, MetadataInitTokenRva = 47237424L, MetadataInitFlagRva = 59849644L)]
	public unsafe AtlasPage Clone()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clone_Public_AtlasPage_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AtlasPage>(intPtr) : null;
	}

	[CallerCount(2392)]
	[CachedScanResults(RefRangeStart = 18875, RefRangeEnd = 21267, XrefRangeStart = 18875, XrefRangeEnd = 21267, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AtlasPage()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AtlasPage>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public AtlasPage(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
