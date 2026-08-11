using System;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;

namespace Il2CppSpine.Collections;

public class OrderedDictionaryDebugView<TKey, TValue> : Il2CppSystem.Object
{
	static OrderedDictionaryDebugView()
	{
		Il2CppClassPointerStore<OrderedDictionaryDebugView<TKey, TValue>>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Collections", "OrderedDictionaryDebugView`2"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[2]
		{
			Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<TKey>.NativeClassPtr)),
			Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<TValue>.NativeClassPtr))
		})).TypeHandle.value);
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<OrderedDictionaryDebugView<TKey, TValue>>.NativeClassPtr);
	}

	public OrderedDictionaryDebugView(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
