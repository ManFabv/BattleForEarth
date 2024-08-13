using System;
using System.Reflection;
using System.Runtime.ConstrainedExecution;

public static class ReflectionUtils
{
    public static TFieldType GetFieldValue<TFieldType>(string fieldName, object instance)
    {
        FieldInfo fieldInfo = instance?.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        return (TFieldType)fieldInfo?.GetValue(instance);
    }

    public static void SetFieldValue<TPropertyType>(string fieldName, object instance, TPropertyType value, BindingFlags bindingAttr = BindingFlags.NonPublic | BindingFlags.Instance)
    {
        FieldInfo fieldInfo = instance?.GetType().GetField(fieldName, bindingAttr);
        fieldInfo?.SetValue(instance, value);
    }

    public static TPropertyType GetPropertyValue<TPropertyType>(string propertyName, object instance)
    {
        PropertyInfo propertyInfo = instance?.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance);
        return (TPropertyType)propertyInfo?.GetValue(instance);
    }

    public static void SetPropertyValue<TPropertyType>(string propertyName, object instance, TPropertyType value, BindingFlags bindingAttr = BindingFlags.NonPublic | BindingFlags.Instance)
    {
        PropertyInfo propertyInfo = instance?.GetType().GetProperty(propertyName, bindingAttr);
        propertyInfo?.SetValue(instance, value);
    }

    public static TMethodReturnType CallPrivateMethodAndGetReturnValue<TMethodReturnType>(string methodName, object instance, object[] parameters = null)
    {
        MethodInfo methodInfo = instance?.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        TMethodReturnType result = (TMethodReturnType)methodInfo.Invoke(instance, parameters);
        return result;
    }

    public static void CallPrivateMethod(string methodName, object instance, object[] parameters = null)
    {
        MethodInfo methodInfo = instance?.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        methodInfo.Invoke(instance, parameters);
    }
}
