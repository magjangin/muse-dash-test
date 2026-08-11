using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppSpine;

public class BoundingBoxAttachment : VertexAttachment
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_Copy_Public_Virtual_Attachment_0;

	static BoundingBoxAttachment()
	{
		Il2CppClassPointerStore<BoundingBoxAttachment>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "BoundingBoxAttachment");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BoundingBoxAttachment>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoundingBoxAttachment>.NativeClassPtr, 100663531);
		NativeMethodInfoPtr_Copy_Public_Virtual_Attachment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoundingBoxAttachment>.NativeClassPtr, 100663532);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 783402, RefRangeEnd = 783403, XrefRangeStart = 783398, XrefRangeEnd = 783402, MetadataInitTokenRva = 46333480L, MetadataInitFlagRva = 59849653L)]
	public unsafe BoundingBoxAttachment(string name)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<BoundingBoxAttachment>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 783403, XrefRangeEnd = 783411, MetadataInitTokenRva = 46333420L, MetadataInitFlagRva = 59849654L)]
	public unsafe override Attachment Copy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Copy_Public_Virtual_Attachment_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Attachment>(intPtr) : null;
	}

	public BoundingBoxAttachment(IntPtr pointer)
		: base(pointer)
	{
	}
}
