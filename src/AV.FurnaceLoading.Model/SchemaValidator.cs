using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AV.FurnaceLoading.Model
{
    public class SchemaValidator : ISchemaValidator
    {
        Collision _collision = new Collision();

        public bool SchemaValidFor(LoadSchema schema, Cassette cassette)
        {
            try
            {
                foreach (var item in schema.Places)
                {
                    if (!ValidOnWall(item, cassette) || !ValidBetweenPlace(schema, item))
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        private RectLoadPlace ReturnNewRectPlace(RectLoadPlace rect, bool betweenPlace)
        {
            int tempWith, tempHeight;
            if (betweenPlace)
            {
                tempWith = rect.Width + SafetyParameters.SafeDistanceBetweenPlaces(rect) * 2;
                tempHeight = rect.Height + SafetyParameters.SafeDistanceBetweenPlaces(rect) * 2;
            }
            else
            {
                tempWith = rect.Width + SafetyParameters.SafeDistanceToWalls(rect) * 2;
                tempHeight = rect.Height + SafetyParameters.SafeDistanceToWalls(rect) * 2;
            }
            return new RectLoadPlace(tempHeight, tempWith, rect.Rotation, rect.Center);
        }

        private CircleLoadPlace ReturnNewCirclePlace(CircleLoadPlace circle, bool betweenPlace)
        {
            int tempDiameter;
            if (betweenPlace)
                tempDiameter = (circle.Diameter/2 + SafetyParameters.SafeDistanceBetweenPlaces(circle)) * 2;
            else
                tempDiameter = (circle.Diameter/2 + SafetyParameters.SafeDistanceToWalls(circle)) * 2;
            return new CircleLoadPlace(tempDiameter, circle.Center);
        }

        /// <summary>
        /// Возвращает новый объект = старый + безопасное растояние
        /// </summary>
        /// <param name="place"></param>
        /// <param name="betweenPlace">true - если между объектами; false - если со стенкой</param>
        /// <returns></returns>
        public LoadPlace LoadPlaceWithSafetyParameter(LoadPlace place, bool betweenPlace)
        {
            switch (place)
            {
                case RectLoadPlace:
                    return ReturnNewRectPlace((RectLoadPlace)place, betweenPlace);
                case CircleLoadPlace:
                    return ReturnNewCirclePlace((CircleLoadPlace)place, betweenPlace);
            }
            return place;
        }

        /// <summary>
        /// Проверка колизии с границами зоны
        /// </summary>
        /// <param name="place"></param>
        /// <param name="cassette"></param>
        /// <returns></returns>
        public bool ValidOnWall(LoadPlace place, Cassette cassette)
        {
            LoadPlace newPlace = LoadPlaceWithSafetyParameter(place, false);
            switch (newPlace)
            {
                case RectLoadPlace:
                    return !_collision.RectVsBox((RectLoadPlace)newPlace, cassette);
                case CircleLoadPlace:
                    return !_collision.CircleVsBox((CircleLoadPlace)newPlace, cassette);
            }
            return false;
        }

        /// <summary>
        /// Проверка колизии между объектами
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="place"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool ValidBetweenPlace(LoadSchema schema, LoadPlace place)
        {
            LoadPlace newPlace = LoadPlaceWithSafetyParameter(place, true);
            foreach (var item in schema.Places)
            {
                if (place != item)
                {
                    if (newPlace is RectLoadPlace && item is RectLoadPlace)
                    {
                        if (_collision.RectVsRectCheck((RectLoadPlace)newPlace, (RectLoadPlace)item))
                            return false;
                    }
                    else if (newPlace is CircleLoadPlace && item is CircleLoadPlace)
                    {
                        if (_collision.CircleVsCircleCheck((CircleLoadPlace)newPlace, (CircleLoadPlace)item))
                            return false;
                    }
                    else if (newPlace is CircleLoadPlace && item is RectLoadPlace)
                    {
                        if (_collision.CircleVsRectCheck((CircleLoadPlace)newPlace, (RectLoadPlace)item))
                            return false;
                    }
                    else if (newPlace is RectLoadPlace && item is CircleLoadPlace)
                    {
                        if (_collision.CircleVsRectCheck((CircleLoadPlace)item, (RectLoadPlace)newPlace))
                            return false;
                    }
                    else
                        throw new NotImplementedException();
                }
            }
            return true;
        }
    }
}
