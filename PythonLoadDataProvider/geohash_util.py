# -*- coding: utf-8 -*-
import geohash2 as geohash

class Geohash2:
    def __init__(self, lat, lon):
        self.lat = lat
        self.lon = lon


    def get_geohash(self, precision=6):
        """
        Devuelve el geohash para las coordenadas dadas.

        :param precision: Precision del geohash (por defecto es 6).
        :return: Geohash como una cadena.
        """
        try:
            return geohash.encode(self.lat, self.lon, precision)
        except Exception as e:
            return "error"

def test_geohash():
    # Coordenadas de ejemplo
    latitude = 40.7128
    longitude = -74.0060

    # Codificar las coordenadas en un geohash
    geo_encoded = geohash.encode(latitude, longitude, precision=6)
    print(f"Geohash codificado: {geo_encoded}")

    # Decodificar el geohash de nuevo a coordenadas
    geo_decoded = geohash.decode(geo_encoded)
    print(f"Coordenadas decodificadas: {geo_decoded}")

    print("Prueba de geohash exitosa")