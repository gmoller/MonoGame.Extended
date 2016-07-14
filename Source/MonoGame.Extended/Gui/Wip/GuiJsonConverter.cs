using System;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MonoGame.Extended.Gui.Wip
{
    public class GuiJsonConverter : JsonConverter
    {
        private readonly GuiJsonConverterService _converterService;

        public GuiJsonConverter(GuiJsonConverterService converterService)
        {
            _converterService = converterService;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(TextureRegion2D))
                return true;

            if (objectType == typeof(BitmapFont))
                return true;

            if (objectType == typeof(GuiThickness))
                return true;

            return objectType == typeof(IGuiDrawable);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(TextureRegion2D))
                return _converterService.GetTextureRegion((string) reader.Value);

            if (objectType == typeof(BitmapFont))
                return _converterService.GetFont((string) reader.Value);

            if (objectType == typeof(GuiThickness))
                return GuiThickness.Parse((string) reader.Value);

            var jObject = JObject.Load(reader);
            var drawbaleType = (string)jObject.Property("Type");

            switch (drawbaleType)
            {
                case "Sprite":
                    return jObject.ToObject<GuiSprite>(serializer);
                case "Text":
                    return jObject.ToObject<GuiText>(serializer);
                default:
                    throw new InvalidOperationException($"Unexpected type {drawbaleType}");
            }
        }
    }
}