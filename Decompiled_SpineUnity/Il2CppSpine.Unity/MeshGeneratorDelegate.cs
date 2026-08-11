using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine.Unity;

public sealed class MeshGeneratorDelegate : Il2CppSystem.MulticastDelegate
{
	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_MeshGeneratorBuffers_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_MeshGeneratorBuffers_AsyncCallback_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0;

	static MeshGeneratorDelegate()
	{
		Il2CppClassPointerStore<MeshGeneratorDelegate>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "MeshGeneratorDelegate");
		NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGeneratorDelegate>.NativeClassPtr, 100664338);
		NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_MeshGeneratorBuffers_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGeneratorDelegate>.NativeClassPtr, 100664339);
		NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_MeshGeneratorBuffers_AsyncCallback_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGeneratorDelegate>.NativeClassPtr, 100664340);
		NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MeshGeneratorDelegate>.NativeClassPtr, 100664341);
	}

	[CallerCount(6192)]
	[CachedScanResults(RefRangeStart = 39733, RefRangeEnd = 45925, XrefRangeStart = 39733, XrefRangeEnd = 45925, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MeshGeneratorDelegate(Il2CppSystem.Object @object, System.IntPtr method)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MeshGeneratorDelegate>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
		*(System.IntPtr**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &method;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 792640, RefRangeEnd = 792645, XrefRangeStart = 792619, XrefRangeEnd = 792640, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Invoke(MeshGeneratorBuffers buffers)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)buffers));
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_MeshGeneratorBuffers_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792645, XrefRangeEnd = 792649, MetadataInitTokenRva = 46352012L, MetadataInitFlagRva = 59827188L)]
	public unsafe virtual Il2CppSystem.IAsyncResult BeginInvoke(MeshGeneratorBuffers buffers, Il2CppSystem.AsyncCallback callback, Il2CppSystem.Object @object)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)buffers));
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)callback);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_MeshGeneratorBuffers_AsyncCallback_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.IAsyncResult>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 45955, RefRangeEnd = 45956, XrefRangeStart = 45955, XrefRangeEnd = 45956, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void EndInvoke(Il2CppSystem.IAsyncResult result)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)result);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MeshGeneratorDelegate(System.IntPtr pointer)
		: base(pointer)
	{
	}

	public static implicit operator MeshGeneratorDelegate(System.Action<MeshGeneratorBuffers> P_0)
	{
		return DelegateSupport.ConvertDelegate<MeshGeneratorDelegate>((System.Delegate)P_0);
	}

	public static MeshGeneratorDelegate operator +(MeshGeneratorDelegate P_0, MeshGeneratorDelegate P_1)
	{
		return ((Il2CppObjectBase)Il2CppSystem.Delegate.Combine(P_0, P_1)).Cast<MeshGeneratorDelegate>();
	}

	public static MeshGeneratorDelegate operator -(MeshGeneratorDelegate P_0, MeshGeneratorDelegate P_1)
	{
		object obj = Il2CppSystem.Delegate.Remove(P_0, P_1);
		if (obj != null)
		{
			obj = ((Il2CppObjectBase)obj).Cast<MeshGeneratorDelegate>();
		}
		return (MeshGeneratorDelegate)obj;
	}
}
