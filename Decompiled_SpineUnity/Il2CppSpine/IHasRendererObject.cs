using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine;

public class IHasRendererObject : Il2CppObjectBase
{
	private static readonly System.IntPtr NativeMethodInfoPtr_get_RendererObject_Public_Abstract_Virtual_New_get_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_RendererObject_Public_Abstract_Virtual_New_set_Void_Object_0;

	public unsafe virtual Il2CppSystem.Object RendererObject
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_RendererObject_Public_Abstract_Virtual_New_get_Object_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_set_RendererObject_Public_Abstract_Virtual_New_set_Void_Object_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static IHasRendererObject()
	{
		Il2CppClassPointerStore<IHasRendererObject>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "IHasRendererObject");
		NativeMethodInfoPtr_get_RendererObject_Public_Abstract_Virtual_New_get_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IHasRendererObject>.NativeClassPtr, 100663523);
		NativeMethodInfoPtr_set_RendererObject_Public_Abstract_Virtual_New_set_Void_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IHasRendererObject>.NativeClassPtr, 100663524);
	}

	public IHasRendererObject(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
