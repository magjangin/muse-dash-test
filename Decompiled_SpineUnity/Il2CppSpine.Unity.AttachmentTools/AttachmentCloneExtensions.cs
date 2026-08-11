using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine.Unity.AttachmentTools;

public static class AttachmentCloneExtensions : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_GetCopy_Public_Static_Attachment_Attachment_Boolean_0;

	static AttachmentCloneExtensions()
	{
		Il2CppClassPointerStore<AttachmentCloneExtensions>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity.AttachmentTools", "AttachmentCloneExtensions");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AttachmentCloneExtensions>.NativeClassPtr);
		NativeMethodInfoPtr_GetCopy_Public_Static_Attachment_Attachment_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AttachmentCloneExtensions>.NativeClassPtr, 100664440);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 794247, RefRangeEnd = 794251, XrefRangeStart = 794241, XrefRangeEnd = 794247, MetadataInitTokenRva = 47239140L, MetadataInitFlagRva = 59827122L)]
	public unsafe static Attachment GetCopy(this Attachment o, bool cloneMeshesAsLinked)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)o);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &cloneMeshesAsLinked;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCopy_Public_Static_Attachment_Attachment_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Attachment>(intPtr) : null;
	}

	public AttachmentCloneExtensions(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
