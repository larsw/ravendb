﻿using System;
using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Raven.Client.Linq;
using Xunit;

namespace Raven.Tests.Spatial
{
	public class SpatialSearch : LocalClientTest
	{
		private class SpatialIdx : AbstractIndexCreationTask<Event>
		{
			public SpatialIdx()
			{
				Map = docs => from e in docs
							  select new {e.Venue, e.Date, _ = SpatialIndex.Generate(e.Latitude, e.Longitude)};

				Index(x => x.Venue, FieldIndexing.Analyzed);
			}
		}

		[Fact]
		public void Can_do_spatial_search_with_client_api()
		{
			using (var store = NewDocumentStore())
			{
				new SpatialIdx().Execute(store);

				using (var session = store.OpenSession())
				{
					session.Store(new Event("a/1", 38.9579000, -77.3572000, DateTime.Now));
					session.Store(new Event("a/2", 38.9690000, -77.3862000, DateTime.Now.AddDays(1)));
					session.Store(new Event("b/2", 38.9690000, -77.3862000, DateTime.Now.AddDays(2)));
					session.Store(new Event("c/3", 38.9510000, -77.4107000, DateTime.Now.AddYears(3)));
					session.SaveChanges();
				}

				WaitForIndexing(store);

				using (var session = store.OpenSession())
				{
					RavenQueryStatistics stats;
					var events = session.Advanced.LuceneQuery<Event>("SpatialIdx")
						.Statistics(out stats)
						.WhereLessThanOrEqual("Date", DateTimeOffset.Now.AddYears(1))
						.WithinRadiusOf(6.0, 38.96939, -77.386398)
						.OrderByDescending(x => x.Date)
						.ToList();

					Assert.NotEqual(0, stats.TotalResults);
				}
			}
		}

		[Fact]
		public void Can_do_spatial_search_with_client_api_addorder()
		{
			using (var store = NewDocumentStore())
			{
				new SpatialIdx().Execute(store);

				using (var session = store.OpenSession())
				{
					session.Store(new Event("a/1", 38.9579000, -77.3572000));
					session.Store(new Event("b/1", 38.9579000, -77.3572000));
					session.Store(new Event("c/1", 38.9579000, -77.3572000));
					session.Store(new Event("a/2", 38.9690000, -77.3862000));
					session.Store(new Event("b/2", 38.9690000, -77.3862000));
					session.Store(new Event("c/2", 38.9690000, -77.3862000));
					session.Store(new Event("a/3", 38.9510000, -77.4107000));
					session.Store(new Event("b/3", 38.9510000, -77.4107000));
					session.Store(new Event("c/3", 38.9510000, -77.4107000));
					session.SaveChanges();
				}

				WaitForIndexing(store);

				using (var session = store.OpenSession())
				{
					var events = session.Advanced.LuceneQuery<Event>("SpatialIdx")
						.WithinRadiusOf(6.0, 38.96939, -77.386398)
						.SortByDistance()
						.AddOrder("Venue", false)
						.ToList();

					var expectedOrder = new[] { "a/2", "b/2", "c/2", "a/1", "b/1", "c/1", "a/3", "b/3", "c/3" };
					for (int i = 0; i < events.Count; i++)
					{
						Assert.Equal(expectedOrder[i], events[i].Venue);
					}
				}
			}
		}
	}
}
