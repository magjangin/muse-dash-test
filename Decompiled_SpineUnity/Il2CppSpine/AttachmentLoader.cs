using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppSpine;

public class AttachmentLoader : Il2CppObjectBase
{
	private static readonly IntPtr NativeMethodInfoPtr_NewRegionAttachment_Public_Abstract_Virtual_New_RegionAttachment_Skin_String_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_NewMeshAttachment_Public_Abstract_Virtual_New_MeshAttachment_Skin_String_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_NewBoundingBoxAttachment_Public_Abstract_Virtual_New_BoundingBoxAttachment_Skin_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_NewPathAttachment_Public_Abstract_Virtual_New_PathAttachment_Skin_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_NewPointAttachment_Public_Abstract_Virtual_New_PointAttachment_Skin_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_NewClippingAttachment_Public_Abstract_Virtual_New_ClippingAttachment_Skin_String_0;

	static AttachmentLoader()
	{
		Il2CppClassPointerStore<AttachmentLoader>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "AttachmentLoader");
		NativeMethodInfoPtr_NewRegionAttachment_Public_Abstract_Virtual_New_RegionAttachment_Skin_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AttachmentLoader>.NativeClassPtr, 100663525);
		NativeMethodInfoPtr_NewMeshAttachment_Public_Abstract_Virtual_New_MeshAttachment_Skin_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AttachmentLoader>.NativeClassPtr, 100663526);
		NativeMethodInfoPtr_NewBoundingBoxAttachment_Public_Abstract_Virtual_New_BoundingBoxAttachment_Skin_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AttachmentLoader>.NativeClassPtr, 100663527);
		NativeMethodInfoPtr_NewPathAttachment_Public_Abstract_Virtual_New_PathAttachment_Skin_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AttachmentLoader>.NativeClassPtr, 100663528);
		NativeMethodInfoPtr_NewPointAttachment_Public_Abstract_Virtual_New_PointAttachment_Skin_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AttachmentLoader>.NativeClassPtr, 100663529);
		NativeMethodInfoPtr_NewClippingAttachment_Public_Abstract_Virtual_New_ClippingAttachment_Skin_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AttachmentLoader>.NativeClassPtr, 100663530);
	}

	[CallerCount(0)]
	public unsafe virtual RegionAttachment NewRegionAttachment(Skin skin, string name, string path)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skin);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(path);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_NewRegionAttachment_Public_Abstract_Virtual_New_RegionAttachment_Skin_String_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<RegionAttachment>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe virtual MeshAttachment NewMeshAttachment(Skin skin, string name, string path)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skin);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(path);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_NewMeshAttachment_Public_Abstract_Virtual_New_MeshAttachment_Skin_String_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MeshAttachment>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe virtual BoundingBoxAttachment NewBoundingBoxAttachment(Skin skin, string name)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skin);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_NewBoundingBoxAttachment_Public_Abstract_Virtual_New_BoundingBoxAttachment_Skin_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<BoundingBoxAttachment>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe virtual PathAttachment NewPathAttachment(Skin skin, string name)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skin);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_NewPathAttachment_Public_Abstract_Virtual_New_PathAttachment_Skin_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<PathAttachment>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe virtual PointAttachment NewPointAttachment(Skin skin, string name)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skin);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_NewPointAttachment_Public_Abstract_Virtual_New_PointAttachment_Skin_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<PointAttachment>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe virtual ClippingAttachment NewClippingAttachment(Skin skin, string name)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skin);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_NewClippingAttachment_Public_Abstract_Virtual_New_ClippingAttachment_Skin_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ClippingAttachment>(intPtr) : null;
	}

	public AttachmentLoader(IntPtr pointer)
		: base(pointer)
	{
	}
}
