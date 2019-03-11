using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Catalogue.XmlSerializer.Writing.ContentWriters
{
    public class ReportEmitHelpers
    {
        public static Func<object, object> EmitReadPropertyFunc(PropertyInfo propertyInfo, Type objectType)
        {
            var getMethod = propertyInfo.GetGetMethod(true);
            var method =
                new DynamicMethod(Guid.NewGuid().ToString(), typeof(object), new[] {typeof(object)}, true);

            if (getMethod == null)
                throw new MissingMemberException(objectType.FullName, propertyInfo.Name + ".get()");
            var il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            if (objectType.IsValueType)
            {
                il.Emit(OpCodes.Unbox, objectType);
                il.Emit(OpCodes.Call, getMethod);
            }
            else
            {
                il.Emit(OpCodes.Castclass, objectType);
                il.Emit(OpCodes.Callvirt, getMethod);
            }
            if (getMethod.ReturnType.IsValueType)
                il.Emit(OpCodes.Box, getMethod.ReturnType);
            il.Emit(OpCodes.Ret);
            return (Func<object, object>)method.CreateDelegate(typeof(Func<object, object>));
        }
    }
}