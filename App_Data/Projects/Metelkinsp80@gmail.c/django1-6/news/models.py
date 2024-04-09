from django.db import models


class Articles(models.Model):
    company = models.CharField('Авиакомпания', max_length=32)
    term = models.CharField('Терминал', max_length=50)
    status = models.CharField('Статус', max_length=32)
    name = models.CharField('Направления', max_length=32)
    time = models.TimeField('Время', max_length=12)

    def __str__(self):
        return self.name

    class Meta:
        verbose_name = 'Онлайн табло'
        verbose_name_plural = 'Онлайн табло'


